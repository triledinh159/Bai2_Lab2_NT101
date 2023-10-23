using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
namespace Lab2_Ex2
{
    public partial class RandomKeyNumeric : Form
    {
        public RandomKeyNumeric()
        {
            InitializeComponent();
        }


        private bool IsPrime(BigInteger n)
        {
            int k = 5;
            if (n <= 1)
                return false;
            if (n <= 3)
                return true;
            if (n % 2 == 0)
                return false;

            BigInteger d = n - 1;
            int s = 0;
            while (d % 2 == 0)
            {
                d /= 2;
                s++;
            }

            for (int i = 0; i < k; i++)
            {
                BigInteger a = GenerateRandomBigInteger(2, n - 2);
                BigInteger x = ModExp(a, d, n);
                if (x == 1 || x == n - 1)
                    continue;

                for (int r = 1; r < s; r++)
                {
                    x = ModExp(x, 2, n);
                    if (x == 1)
                        return false;
                    if (x == n - 1)
                        break;
                }

                if (x != n - 1)
                    return false;
            }

            return true;
        }

        private BigInteger ModExp(BigInteger a, BigInteger x, BigInteger p)
        {
            BigInteger result = 1;
            a = a % p;

            while (x > 0)
            {
                if (x % 2 == 1)
                    result = (result * a) % p;

                x >>= 1; // Equivalent to x = x / 2
                a = (a * a) % p;
            }

            return result;
        }

        private BigInteger GenerateRandomBigInteger(BigInteger minValue, BigInteger maxValue)
        {
            if (minValue > maxValue)
            {
                throw new ArgumentException("minValue must be less than or equal to maxValue");
            }

            Random random = new Random();
            byte[] bytes = new byte[maxValue.ToByteArray().Length];
            BigInteger randomNum;

            do
            {
                random.NextBytes(bytes);
                randomNum = new BigInteger(bytes);
            } while (randomNum < minValue || randomNum > maxValue);

            return randomNum;
        }

        private BigInteger GCD(BigInteger a, BigInteger b)
        {
            if (b == 0) return a;
            return GCD(b, a % b);
        }

        private BigInteger Mod(BigInteger a, BigInteger b, BigInteger c)
        {
            BigInteger x = 1, y = a;
            while (b > 0)
            {
                if (b % 2 == 1)
                {
                    x = (x * y) % c;
                }
                y = (y * y) % c;
                b /= 2;
            }
            return (BigInteger)(x % c);
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox2.Clear();
            richTextBox3.Clear();
            richTextBox4.Clear();
            richTextBox5.Clear();

            Random random = new Random();
            BigInteger p, q;

            do
            {
                p = 1000 + random.Next(9000);
            } while (!IsPrime(p));

            do
            {
                q = 1000 + random.Next(9000);
            } while (!IsPrime(q));

            BigInteger n = p * q;
            BigInteger phi = (p - 1) * (q - 1);

            BigInteger publicKey = 0;
            for (BigInteger i = 2; i < phi; i++)
            {
                if (GCD(i, phi) == 1)
                {
                    publicKey = i;
                    break;
                }
            }

            BigInteger privateKey = 2;
            while (true)
            {
                if ((privateKey * publicKey - 1) % phi == 0)
                {
                    break;
                }
                privateKey++;
            }

            richTextBox1.AppendText(p + Environment.NewLine);
            richTextBox2.AppendText(q + Environment.NewLine);
            richTextBox3.AppendText(n + Environment.NewLine);
            richTextBox4.AppendText(publicKey + Environment.NewLine);
            richTextBox5.AppendText(privateKey + Environment.NewLine);
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            richTextBox7.Clear();

            BigInteger plalongext = BigInteger.Parse(richTextBox6.Text);
            BigInteger publicKey = BigInteger.Parse(richTextBox4.Text.Trim());
            BigInteger n = BigInteger.Parse(richTextBox3.Text.Trim());

            BigInteger encrypted = Mod(plalongext, publicKey, n);
            richTextBox7.AppendText(encrypted + Environment.NewLine);
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            richTextBox6.Clear();
            BigInteger ciphertext = BigInteger.Parse(richTextBox7.Text);
            BigInteger privateKey = BigInteger.Parse(richTextBox5.Text.Trim());
            BigInteger n = BigInteger.Parse(richTextBox3.Text.Trim());

            BigInteger decrypted = Mod(ciphertext, privateKey, n);
            richTextBox6.AppendText(decrypted + Environment.NewLine);
        }
    }

}
