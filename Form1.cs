using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.IO;


namespace Диплом
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //начальные значения таймера + выходные параметры
        int nc, i, il, oshib, shift = 0, caps, mili2, mili = 0;
        int chislo_polsov = 0; // записывается в log
        int it = 0, popitki = 0;

        double[] mili_vv = new double[200];
        double vrem_vvod = 0, skor = 0;

        string path = @"database.txt"; //путь с названием файла собранных данных
        string nach_str = "";
        string logp = @"log.ini", rasdel = ";"; // Различные данные для сохранения программой параметров
 


        
        
        //подпрограмма дробление строки на отдельные элементы (типа char)
        void char_clovo()
        {
            string s = label4.Text;
            string ss = s;
            char[] mas = new char[s.Length];
            int dli = s.Length-1;
            for (int i = 0; i < s.Length; i++)
            {
                
                mas[i] = Convert.ToChar(ss.Remove(1,dli-i));
                ss = ss.Substring(ss.Length - (dli-i));
            }}

        void sbros() // Приводит программу в первоначальное состояние
        {
            label4.Text = "";
            textBox3.Enabled = true;
            button11.Enabled = true;
            textBox2.Text = "";
            textBox2.Enabled = true;
            button1.Enabled = false;
            button3.Enabled = true;
            textBox1.Enabled = false;
            label8.Text = "0";
            popitki = 0;
            listBox2.Items.Clear();
            //очистка
            listBox1.Items.Clear();
            textBox1.Text = "";
            timer1.Enabled = false;
            mili = 0; il = 0; oshib = 0; shift = 0; caps = 0; mili2 = 0; //обнуления для сброса значений
            
            File.Delete(path);
        }



        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (il==0)
                listBox1.Items.Add("Пользователь: " + textBox2.Text);

            il++;

            //Динамика ввода
            if (dinamic.Checked == true)
            {
                
                int col;

                string s = textBox1.Text;
                col = s.Length;
                string[] Mas_sk = new string[100];
                string s2 = "";
                if (col > 1)
                {
                    //выводит две последние буквы, между которыми каждый раз фиксируется время
                    s2 = s.Substring(s.Length - 2);
                    Mas_sk[i] = s2;
                   
                    if (nc > 0)
                    {
                        
                            
                        listBox1.Items.Add(Convert.ToString(mili)
                            + "мс между'" + Mas_sk[i].Remove(1, 1) + "' и '" + Mas_sk[i].Remove(0, 1) + "'");
                        
                    
                        nach_str = nach_str + "мс между'" + Mas_sk[i].Remove(1, 1) + "' и '" + Mas_sk[i].Remove(0, 1) + "'" + rasdel;
                        mili_vv[it] = mili;
                    }
                    i++;
                    if (i == 100)
                        i = 0;
                        
                    it++;
                }
                nc++;


                //перезапуск таймера при изменение в textBox1 значения
                mili = 0;
                timer1.Enabled = false;
                timer1.Enabled = true;
            }
       

            //Скорость ввода
            if (skorost.Checked == true)
            {

                if (textBox1.Text.Length <= label4.Text.Length)
                {
                    vrem_vvod = vrem_vvod + mili2;
                    vrem_vvod = mili2;
                    timer1.Enabled = true;
                }
                else
                {
                    timer1.Enabled = false;
                }
                if (textBox1.Text == label4.Text)
                {
                    nach_str = nach_str + "Скорость ввода (кл/мс)" + rasdel;
                    skor = Math.Round(textBox1.Text.Length / vrem_vvod, 3);
                    listBox1.Items.Add(Convert.ToString(skor) + " кл/мс");
                }}
            //кол-во ошибок при вводе слова
            if (checkBox1.Checked == true)
            {
                if (textBox1.Text.Length == label4.Text.Length)
                {
                    listBox1.Items.Add("Кол-во ошибок: " + oshib);
                    nach_str = nach_str + "Кол-во ошибок" + rasdel;
                }}
            //использование caps и shift
            if (checkBox2.Checked == true)
            {
                if (textBox1.Text.Length == label4.Text.Length)
                {
                    nach_str = nach_str + "Shift" + rasdel + "Caps" + rasdel;
                    listBox1.Items.Add("Shift использовали (раз):" + shift);
                    listBox1.Items.Add("Caps использовали (раз):" + caps);
                }}


            //запись полученных данных в файл
            if (textBox1.Text.Length == label4.Text.Length)
            {
               
                button1.Enabled = true;
                string line="";
                string lii = "";
                nach_str = "Пользователь" + rasdel + nach_str;


                //проверка наличия файла
                if (File.Exists(path) == false)
                {

                    FileInfo FF = new FileInfo(path);
                    if (FF.Exists == false)
                    {
                        FileStream fs = FF.Create();
                        fs.Close();
                    }}

                using (StreamReader ss = new StreamReader(path, System.Text.Encoding.Default))
                {
                    while ((line = ss.ReadLine()) != null)
                    {
                        lii = line;
                        break;
                    }}

                if (lii.Contains(nach_str) == false)
                {
                    using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
                    {
                        sw.WriteLine(nach_str);
                        lii = nach_str;
                    }}

                if (lii.Contains(nach_str) == true)
                {

                    string buff = "";

                    buff = "";
                    for (int iu = 0; iu < label4.Text.Length - 1; iu++)
                        buff = buff + Convert.ToString(mili_vv[iu])+ rasdel;

                    it = 0;
                    using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default))
                    {
                        string rs1 = rasdel, rs2 = rs1, rs3 = rs2,rs4=rs3;
                        string sk, oshi, shif, cap;
                        sk = Convert.ToString(skor);
                        oshi = Convert.ToString(oshib);
                        shif = Convert.ToString(shift);
                        cap = Convert.ToString(caps);


                        if (dinamic.Checked == false)
                        {
                            buff = "";
                        }
                        if (skorost.Checked == false)
                        {
                            sk = "";
                            rs2 = "";
                           
                        }
                        if (checkBox1.Checked == false)
                        {
                            oshi = "";
                            rs3 = "";
                            
                        }
                        if (checkBox2.Checked == false)
                        {
                            shif = ""; cap = "";
                            rs4 = "";

                        }
                        //допись информационных данных для нейросети
                        int bf;
                        string per="";
                        bf = chislo_polsov;
                        while (chislo_polsov != -1)
                        {
                            if (chislo_polsov - 1 == -1)
                                    per = per + ";" + "1";

                            if (chislo_polsov > 0)
                                per = per + ";0";

                            chislo_polsov--;
                        }
                        chislo_polsov = bf;
                        sw.WriteLine(textBox2.Text+rs1 + buff +sk + rs2 + oshi
                            + rs3 + shif + rs4 + cap+per);
                    }
                    nach_str = "";
                }}}

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            // таймер для измерения времени между нажатиями клавиш
            mili++; // для динамики
            mili2++; // для скорости

        }

        private void button1_Click(object sender, EventArgs e)
        {
            popitki++;
            button1.Enabled = false;
            
            label8.Text = Convert.ToString(popitki/2);
            if (popitki/2== 10)
            {
                
                //записывает кол-во пользователей, прошедших процедуру снятия КП
                 using (StreamWriter v_log = new StreamWriter(logp, false, System.Text.Encoding.Default))
                {
                    chislo_polsov++;
                    v_log.WriteLine(label4.Text);
                    v_log.WriteLine(chislo_polsov);
                        
                }
                
                using (StreamReader ss = new StreamReader(logp, System.Text.Encoding.Default))
                {
                    string line;
                    int i = 0;
                    while ((line = ss.ReadLine()) != null)
                    {

                        if(i==1)
                        {
                            chislo_polsov = Convert.ToInt32(line);
                            i = 0;
                            break;
                        }
                        i++;
                    }
                }
                string sav;
                using (StreamReader buf = new StreamReader(path, System.Text.Encoding.Default))
                {
                    
                    int i=0;
                    sav = buf.ReadToEnd();
                    i = sav.IndexOf("\n") - 1;
                    if (chislo_polsov==1)
                    sav = sav.Remove(i) +textBox2.Text + sav.Remove(0, i);
                    else
                        sav = sav.Remove(i) + ";" + textBox2.Text + sav.Remove(0, i);

                }
                using (StreamWriter zapis = new StreamWriter(path, false, System.Text.Encoding.Default))
                {
                    zapis.Write(sav);

                }
                /// Добавление к каждой строке нулей

     
                using (StreamReader buf = new StreamReader(path, System.Text.Encoding.Default))
                {
                    if (chislo_polsov >1)
                    {
                        int i = 0;
                        sav = buf.ReadToEnd();
                        i = sav.IndexOf("\n") + 2; //уход с шапки
                        for (int t = 0; t < (chislo_polsov-1)*10; t++)
                        {
                          
                            i = sav.IndexOf("\n", i+2);
                            sav = sav.Insert(i-1, ";0");
                            i = i + 2; //плюс записанный текст ;0
                          
                        }
                       }
                                     

                }
                using (StreamWriter zapis = new StreamWriter(path, false, System.Text.Encoding.Default))
                {
                    zapis.Write(sav);

                }
                

                textBox1.Enabled = false;
                textBox2.Enabled = true;
                button3.Enabled = true;
                textBox2.Text = "";
                popitki = 0;
                
            }
            
            //при достижении необходимой фразы, появляется возможность повторить процедуру

            if (skorost.Checked == true || dinamic.Checked == true||checkBox1.Checked==true ||checkBox2.Checked==true)
            {
                if (textBox1.Text == label4.Text)
                {
                    il = -1;
                    listBox1.Items.Add("---------------------------------------");
                    textBox1.Text = "";
                    timer1.Enabled = false;
                    mili = 0;  oshib = 0; shift = 0; caps = 0; mili2 = 0;
            }}}

        
        private void button4_Click(object sender, EventArgs e)
        {
            listBox1.Visible = false;
            button10.Visible = true;
            button9.Visible = false;
            listBox2.Visible = true;
            listBox2.Items.Clear();
            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        listBox2.Items.Add(line);
                    }}}
            button5.Visible = true;
        }
      

       
        private void button5_Click(object sender, EventArgs e)
        {
            listBox2.Visible = false;
            button5.Visible = false;
        }
       
        private void button6_Click(object sender, EventArgs e)
        {

            DialogResult result=MessageBox.Show("При изменение настроек произойдет утеря собранных данных." +
                " Согласиться на УДАЛЕНИЕ ДАННЫХ и изменение настроек?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result==DialogResult.Yes)
            {
                sbros();
                chislo_polsov = 0;
                panel1.Visible = false;
                groupBox1.Visible = true;
            }

        }
        private void button7_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            panel1.Visible = true;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (File.Exists(path))
            {
                string sav = "";
                using (StreamReader cg = new StreamReader(path, System.Text.Encoding.Default))
                {
                    sav = cg.ReadToEnd();
                }

                if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;
                // получаем выбранный файл
                string filename = saveFileDialog1.FileName;
                // сохраняем текст в файл
                System.IO.File.WriteAllText(filename, sav,System.Text.Encoding.Default);
                MessageBox.Show("Файл сохранен", "Окно сообщений", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Файл отсутсвует", "Окно сообщений", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

     

        private void button10_Click(object sender, EventArgs e)
        {
            listBox2.Visible = false;
            button5.Visible = false;
            listBox1.Visible = true;
            button10.Visible = false;
            button9.Visible = true;
            button9.Width = 124;
            button9.Height = 23;
        }
        private void button9_Click(object sender, EventArgs e)
        {
            
            listBox1.Visible = false;
            button10.Visible = true;
            button9.Visible = false;

        }
        private void button3_Click(object sender, EventArgs e)
        {
            string svv = "";
            if (File.Exists(path))
            {
                using (StreamReader cg = new StreamReader(path, System.Text.Encoding.Default))
                {
                    svv = cg.ReadToEnd();
                }
            }
            if (svv.Contains(textBox2.Text+";") == false && textBox2.Text != "" && textBox2.Text != null)
            {

                textBox2.Enabled = false;
                if (button11.Enabled==false)
                textBox1.Enabled = true;

                button3.Enabled = false;
                label8.Text = "0"; popitki = 0;

            }
            else
                MessageBox.Show("Такой пользователь в базе уже есть\nили имя еще не задано!", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private void button11_Click(object sender, EventArgs e)
        {
            if (textBox3.Text.Length > 2 && textBox3.Text.Length < 36)
            {
                button11.Enabled = false;
                textBox3.Enabled = false;
                if (button3.Enabled == false)
                    textBox1.Enabled = true;
                label4.Text = textBox3.Text;
                File.Delete(logp);
                using (StreamWriter sw = new StreamWriter(logp, true, System.Text.Encoding.Default))
                {
                    sw.WriteLine(textBox3.Text);
                    sw.WriteLine(0);
                }


            }
            else
                MessageBox.Show("Кол-во букв текста должно быть не меньше 3 и не больше 35(25)", "Окно сообщений", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

        }
        private void button12_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Для того, чтобы поменять слово,\nнеобходимо удалить старые данные!\n\n! ! !  " +
                "Если нет необходимости менять вводимый текст,\nто данный шаг можно пропустить.", "Окно сообщений", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Form2 newForm = new Form2();
            newForm.Show();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(logp) == false)
            {
                using (StreamWriter sws = new StreamWriter(logp, true, System.Text.Encoding.Default))
                {
                    sws.WriteLine("");
                    sws.WriteLine(0);
                }
            }


            if (File.Exists(path))
            {
               
                using (StreamReader sr = new StreamReader(logp, System.Text.Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        label4.Text = line;
                        textBox3.Text = line;
                        break;
                    }

                    label4.Text = textBox3.Text;
                }
            }

            if (File.Exists(path)==false)
            {
                
                textBox3.Enabled = true;
                button11.Enabled = true;
            }
            

            listBox1.Size = new Size(245,173);
            listBox2.Size = new Size(245, 173);
           
            button1.Click += button1_Click;
           
           
            saveFileDialog1.Filter = "Text files(*.txt)|*.txt";
            saveFileDialog1.FileName = "database";

            button5.Width = 124;
            button5.Height = 23;
            
            listBox2.Visible = false;

            textBox1.MaxLength = label4.Text.Length;
            char_clovo();

            using (StreamReader ss = new StreamReader(logp, System.Text.Encoding.Default))
            {
                string line;
                int i = 0;
                while ((line = ss.ReadLine()) != null)
                {

                    if (i == 1)
                    {
                        chislo_polsov = Convert.ToInt32(line);
                        i = 0;
                        break;
                    }
                    i++;
                }
            }

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

            string s = label4.Text;
            string ss = s;
            char[] mas = new char[s.Length];
            int dli = s.Length - 1;
            for (int i = 0; i < s.Length; i++)
            {

                mas[i] = Convert.ToChar(ss.Remove(1, dli - i));
                ss = ss.Substring(ss.Length - (dli - i));
            }
            //обеспечить активацию Enter
            if (label4.Text.Length == textBox1.Text.Length)
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    button1.PerformClick();
                }
            }


            if (label4.Text.Length + 1 != textBox1.Text.Length)
            {
                if (label4.Text.Length == textBox1.Text.Length)
                    il--;
                if ((e.KeyChar == (char)Keys.Back) || (e.KeyChar != mas[il]) || ((e.KeyChar != mas[il])&&e.KeyChar == (char)Keys.Delete) || (e.KeyChar == (char)Keys.Enter)
                    )
                {
                    e.Handled = true;
                    oshib++;
                }
                if (label4.Text.Length == textBox1.Text.Length)
                    il++;
                else
                {
                    //Проверка нажатия Shift
                    if (Control.ModifierKeys == Keys.Shift)
                    {
                        e.Handled = false;
                        shift++;
                    }
                }
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            // Была ли использована клавиша Caps Lock

            if (e.KeyCode == Keys.CapsLock)
            {
                e.Handled = false;
                caps = 1;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы уверены, что хотите удалите ВСЕ СОБРАННЫЕ ДАННЫЕ?", "Внимание",
                MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
                sbros();
            chislo_polsov = 0;
                
        }}}
