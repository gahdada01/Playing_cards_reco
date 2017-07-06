using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using AForge.Imaging.Filters;
using PlayingCardRecognition;

namespace PlayingCardRecognition_SampleImages
{
    public partial class p1S : Form
    {
        private Pen pen = new Pen(Brushes.Orange, 4); //is used for drawing rectangle around card
        private Font font = new Font("Tahoma", 15, FontStyle.Bold); //is used for writing string on card
        Boolean btnClick = true;
        private int[] rankH = new int[5];
        private string[] suitH = new string[5];
        private int counter2 = 0,counter3 =0;
        private int[] scoresHolder = new int[4];

        private int fileNameCounter = 10;
        private CardRecognizer recognizer = new CardRecognizer();
        private Bitmap image,im1,im2,im3;

        public p1S()
        {
            InitializeComponent();
            //btn_Next_Click(this, null);
        }
        /*
        private void btn_Next_Click(object sender, EventArgs e)
        {
            fileNameCounter++;
            if (fileNameCounter > 10)
                fileNameCounter = 1; //Rewind

            this.LoadImage();
            this.ProcessRecognition();
        }
        *///replace btn_Next_Click

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            browseImage1.ShowDialog();
            
            this.im1 = new Bitmap(browseImage1.OpenFile());
            
            pb1.Image = ResizeBitmap(this.im1);
            btnClick = true;
            
        }


        private void btnBrowse2_Click(object sender, EventArgs e)
        {
            browseImage1.ShowDialog();
            this.im2 = new Bitmap(browseImage1.OpenFile());
            pb2.Image = ResizeBitmap(this.im2);
            btnClick = true;
        }


        private void btnBrowse3_Click(object sender, EventArgs e)
        {
            browseImage1.ShowDialog();
            this.im3 = new Bitmap(browseImage1.OpenFile());
            pb3.Image = ResizeBitmap(this.im3);
            btnClick = true;
        }


        private void btnProcess_Click(object sender, EventArgs e)
        {
            if (btnClick==true)
            {
                this.ProcessRecognition();
            }
            btnClick = false;
        }
        /*
        private void LoadImage()
        {
            string fileName = fileNameCounter.ToString() + ".png";
            string imagePath = @"Sample Images\" + fileName;
            this.image = Bitmap.FromFile(imagePath) as Bitmap;
            this.pb_loaded.Image = ResizeBitmap(this.image);
            lbl_FileName.Text = "File Name : " + fileName;
        }*/

        private Bitmap ResizeBitmap(Bitmap bmp)
        {
            ResizeBilinear resizer = new ResizeBilinear(pb1.Width, pb1.Height);

            return resizer.Apply(bmp);
        }

        private int getScore()
        {
            int checker = 0,checker2 =0, counter =0;
            int[] holder = new int[4] {-1,-1,-1,-1};
            int[] holder2 = new int[4] { -1, -1, -1, -1};
            int[] rankH_t = new int[5];
            string[] suitH_t = new string[4];
            int y = 0;
            int finalScore=0;
            int score1 = 0;
            int score2 = 0;

            for (int i = 0; i <= 3; i++)
            {
                rankH[i] = recognizer.rankCard[i];
                suitH[i] = recognizer.suitCard[i];
                rankH_t[i] = rankH[i];
                suitH_t[i] = suitH[i];

            }

            for(int i = 0; i<=3;i++)
            {
                rankH[i] = recognizer.rankCard[i];
                suitH[i] = recognizer.suitCard[i];
                //txtCards.Clear();
                y = 0;
                for (int z = 0; z <= 3; z++)
                {
                    
                    if (z != i) //avoid comparing its own
                    {
                        if (suitH[i] == suitH_t[z]) 
                        {
                            for (int c = 0; c <= 3; c++) //check compared card has not been used
                            {

                                    if (holder[c] != i)
                                    {
                                        checker = 0;
                                    }
                                    else
                                    {
                                        checker = 1;
                                        c = 4;
                                    }

                                    if (c != 4)
                                    {
                                        if (holder2[c] != i)
                                        {
                                            checker = 0;
                                        }
                                        else
                                        {
                                            checker = 1;
                                            c = 3;
                                        }
                                    }
                                
                            }
                            if (checker == 0) //enter if compared card has not been used
                            {

                                for (int c = 0; c <= 3; c++) //check if if add to current score or new score
                                {
                                    if (holder[c] != -1)
                                    {

                                        if (suitH[i] == suitH[holder[c]])
                                        {
                                            checker2 = 0;
                                            c = 4;
                                        }
                                        else
                                        {

                                            checker2 = 1;
                                            
                                        }
                                    }
                                    else
                                    {
                                        if (checker2 != 1)
                                        {

                                            checker2 = 0;
                                        }
                                    }
                                }
                                if (checker2 == 0)
                                {
                                    if (counter != 3)
                                    {
                                        score1 = rankH[i] + rankH[z];
                                        holder[y] = z;
                                        y++;
                                        counter = 3;
                                    }
                                    else
                                    {
                                        score1 = score1 + rankH[z];
                                        holder[y] = z;
                                        y++;
                                    }
                                }
                                else
                                {
                                    score2 = score2+rankH[i] + rankH[z];
                                    holder2[y] = z;
                                }

                            }                           
                            
                        }

                    }
                    
                }
                //txtCards.AppendText(rankH.ToString() + Environment.NewLine);
            }
            
            //record the highest score accumulated
            if (score1 >= score2)
            {
                finalScore = score1;
            }
            else
            {
                finalScore = score2;
            }

            //store and show score according to player#
            if (counter2 == 0)
            {
                scoresHolder[0] = finalScore;
                p1Score.Text = finalScore.ToString();
            }
            else if (counter2 == 1)
            {
                scoresHolder[1] = finalScore;
                p2Score.Text = finalScore.ToString();
            }
            else
            {
                scoresHolder[2] = finalScore;
                p3Score.Text = finalScore.ToString();
            }
            
            /*
            txtBlack.Text = score1.ToString();
            txtRed.Text = score2.ToString(); */
            counter2++;

            return finalScore;
        }
        private string getWinner()
        {
            int playerNum = 0;
            int draw = 0;
            int chck = 0, chck2 = 0, chck3 = 1, chck4 = 0;
            string messageDraw = "Draw!!";
            string result;
            int maxVal = -1;
            int maxVal2 = 0;
            int[] draw3 = new int[3] {-1,-1,-1};
            int[] draw2 = new int[3];
            int s = 0;

            for (int i = 0; i <=2; i++)
            {
                if (scoresHolder[i] >= maxVal)
                {
                    if (scoresHolder[i] > maxVal)
                    {
                        maxVal = scoresHolder[i];
                        maxVal2 = i;
                        chck = 1;
                    }
                    if(chck!=1)
                    {
                        if (scoresHolder[i] == maxVal)
                        {
                            draw = scoresHolder[i];
                            draw3[s] =scoresHolder[s];
                            draw2[s] = i;
                            s++;
                            if (chck2 == 0)
                            {
                                draw2[s] = i - 1;
                                draw3[s] = scoresHolder[i - 1];
                                s++;
                                chck2 = 1;
                            }
                        }
                    }
                    chck = 0;
               }

            }
            if (draw >= maxVal)
            {             
                int g = 0;
                for(int i =0;i<=2;i++)
                {
                    if (chck3==1)
                    {
                        if (chck4 == 0)
                        {
                            if (draw3[0] == draw3[1] && draw3[0] == draw3[2])
                            {
                                winTxt.Text = "Draw!!";
                                //chck3 = 1;
                                chck4 = 1;
                                i = 3;
                            }
                           
                        }
                        if (chck4 != 1)
                        {
                            g = draw2[i] + 1;
                            winTxt.Text = "1P" + g + " ".ToString();
                            chck4 = 1;
                        }
                        
                    }
                    else
                    {
                        //if (chck4 != 1)
                        //{
                            g = draw2[i] + 1;
                            winTxt.Text = "2P" + g + " ".ToString();
                            chck3 = 0;
                        //}
                    }
                } 
            }
            else
            {
                winTxt.Text = "P"+(maxVal2 + 1).ToString();
                //result = (playerNum + 1).ToString();
            }
            

            //winTxt.Text = result.ToString();

            return "";
        }

        private void ProcessRecognition()
        {

            for (int i = 0; i <= 2; i++)
            {
                if (counter3 == 0)
                {
                    this.image = im1;
                }
                else if (counter3 == 1)
                {
                    this.image = im2;
                }
                else
                {
                    this.image = im3;
                }

                CardCollection cards = recognizer.Recognize(this.image);
                //cardImagePanel.DrawImages(cards.ToImageList());
                String r, b;
                //txtCards.Clear();

                //foreach (Card card in cards)
                //{

                //txtCards.AppendText(card.ToString() + Environment.NewLine);

                getScore();

                //getWinner();

                //}
                // */
                //Draw Rectangle around cards and write card strings on card
                using (Graphics graph = Graphics.FromImage(image))
                {
                    foreach (Card card in cards)
                    {
                        graph.DrawPolygon(pen, card.Corners); //Draw a polygon around card
                        PointF point = CardRecognizer.GetStringPoint(card.Corners); //Find Top left corner
                        point.Y += 10;
                        graph.DrawString(card.ToString(), font, Brushes.White, point); //Write string on card
                    }
                }
                /*
                r = recognizer.rank1.ToString();
                b = recognizer.numBlack.ToString();
                txtRed.Text = r;
                txtBlack.Text = b;
                */
                /*
                recognizer.numRed = 0;
                recognizer.numBlack = 0;
                */
                if (counter3 == 0)
                {
                    pb1.Image = ResizeBitmap(this.image);
                }
                else if (counter3 == 1)
                {
                    pb2.Image = ResizeBitmap(this.image);
                }
                else
                {
                    pb3.Image = ResizeBitmap(this.image);
                }


                //this.pb_loaded.Image = ResizeBitmap(this.image);
                counter3++;
            }
            getWinner();
        }

        private void txtCards_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
    }  
}
