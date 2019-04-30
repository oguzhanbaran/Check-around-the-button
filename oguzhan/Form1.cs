using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace oguzhan
{
    public partial class Form1 : Form
    {
        private const int BUTTON_COUNT = 30;

        private bool randomInitialized = false;

        private Button lastActiveButton = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < BUTTON_COUNT; i++)
            {
                Button btn = new Button();
                btn.Size = new Size(40, 35);
                btn.Name = "button_" + i;
                btn.BackColor = SystemColors.Control;
                btn.Tag = i;
                btn.Click += Btn_Click;

                pnlButtons.Controls.Add(btn);
            }
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            Button button = (Button) sender;
            if (button.Text.Length <= 0) return;

            foreach (Button btn in pnlButtons.Controls)
            {
                btn.BackColor = SystemColors.Control;
            }

            button.BackColor = Color.Green;
            lastActiveButton = button;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tmr.Stop();
            randomInitialized = false;
            lastActiveButton = null;
            foreach (Button btn in pnlButtons.Controls)
            {
                btn.Text = "";
                btn.BackColor = SystemColors.Control;
            }
        }

        private void btnRandom_Click(object sender, EventArgs e)
        {
            if (tmr.Enabled) return;
            int min = Convert.ToInt32(nudMin.Value);
            int max = Convert.ToInt32(nudMax.Value);
            if (min > max)
            {
                MessageBox.Show("Minimum değer maksimum değerden büyük olamaz!");
                return;
            }
            if ((max - min) < BUTTON_COUNT)
            {
                MessageBox.Show("2 değer arasındaki fark minimum " + BUTTON_COUNT + " olmalıdır!");
                return;
            }

            Random rnd = new Random();
            List<int> randomNumbers = new List<int>();
            for (int i = 0; i < BUTTON_COUNT; i++)
            {
                int random = rnd.Next(min, max);
                while (randomNumbers.IndexOf(random) > -1)
                {
                    random = rnd.Next(min, max);
                }
                randomNumbers.Add(random);
            }

            for (int i = 0; i < BUTTON_COUNT; i++)
            {
                int randomNumber = randomNumbers[i];
                pnlButtons.Controls["button_" + i].Text = randomNumber.ToString();
            }

            randomInitialized = true;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!randomInitialized)
            {
                MessageBox.Show("İlk önce random sayıları üretin!");
                return;
            }
            if (lastActiveButton == null)
            {
                MessageBox.Show("Lütfen başlangıç noktası seçin!");
                return;
            }

            tmr.Interval = Convert.ToInt32(nudSleep.Value);
            tmr.Start();
        }

        private void tmr_Tick(object sender, EventArgs e)
        {
            List<String> nearbyButtons = new List<String>();

            int activeIndex = Convert.ToInt32(lastActiveButton.Tag);
            if ((activeIndex - 6) > -1)
            {
                Button btn = (Button) pnlButtons.Controls["button_" + (activeIndex - 6)];
                if (btn.BackColor != Color.Red && btn.BackColor != Color.Green)
                {
                    nearbyButtons.Add("button_" + (activeIndex - 6));//Üstteki komşu
                }
            }
            if ((activeIndex + 6) < BUTTON_COUNT)
            {
                Button btn = (Button)pnlButtons.Controls["button_" + (activeIndex + 6)];
                if (btn.BackColor != Color.Red && btn.BackColor != Color.Green)
                {
                    nearbyButtons.Add("button_" + (activeIndex + 6));//Alttaki komşu
                }
            }
            if ((activeIndex - 1) > -1 && (activeIndex % 6) != 0)
            {
                Button btn = (Button)pnlButtons.Controls["button_" + (activeIndex - 1)];
                if (btn.BackColor != Color.Red && btn.BackColor != Color.Green)
                {
                    nearbyButtons.Add("button_" + (activeIndex - 1));//Soldaki komşu
                }
            }
            if ((activeIndex + 1) < BUTTON_COUNT && ((activeIndex + 1) % 6) != 0)
            {
                Button btn = (Button)pnlButtons.Controls["button_" + (activeIndex + 1)];
                if (btn.BackColor != Color.Red && btn.BackColor != Color.Green)
                {
                    nearbyButtons.Add("button_" + (activeIndex + 1));//Sağdaki komşu
                }
            }

            int biggest = -1;
            Button biggestButton = null;
            foreach (String btn in nearbyButtons)
	        {
                int buttonVal = Convert.ToInt32(pnlButtons.Controls[btn].Text);
                if (biggest == -1 || buttonVal > biggest)
                {
                    biggest = buttonVal;
                    biggestButton = (Button) pnlButtons.Controls[btn];
                }
	        }

            if (biggestButton == null)
            {
                tmr.Stop();
            }
            else
            {
                biggestButton.BackColor = Color.Red;
                lastActiveButton = biggestButton;
            }
        }
    }
}
