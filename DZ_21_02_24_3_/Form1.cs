using System.Threading;

namespace DZ_21_02_24_3_
{
    public partial class Form1 : Form
    {
        private Semaphore semaphore;
        public Form1()
        {
            InitializeComponent();

            semaphore = new Semaphore(2, 2, "LimitedInstancesAppSemaphore");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (semaphore.WaitOne(0))
            {
                Form1 newForm = new Form1();
                newForm.Show();
            }
            else
            {               
                MessageBox.Show("Перевищено максимальну кількість дозволених запусків програми.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            semaphore.Release();
        }
    }
}
    

