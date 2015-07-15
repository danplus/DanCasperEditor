using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Dan_Casper_Editor
{
    public partial class Form1 : Form
    {
        byte TransparentColor = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = Version("Dan Casper Editor", 2);
                z.FirstLoad = true;
                LoadPaletteList();
                ShowPalette(comboBox_Palette.Text);
                SetTransparent();
                z.PrjGhostSize = new System.Drawing.Size(8, 16);
                //z.PrjGhostSize.Height = 2;
                LoadData("dane.dce");
                ShowGhost();
                //GenereteFirstFrame();
                ShowFrames();
                z.FirstLoad = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd wykonania funkcji " + FunctionName() + "\r\n\r\n" + ex.ToString());
                //throw new Exception("Błąd wykonania funkcji " + FunctionName() + "\r\n\r\n", ex);
            }
        }

        private void LoadPaletteList()
        {
            string[] filePaths = Directory.GetFiles(@"palette\");
            comboBox_Palette.Items.Clear();
            foreach (string item in filePaths)
            {
                comboBox_Palette.Items.Add(item.Replace(@"palette\",""));
            }
            comboBox_Palette.SelectedIndex = comboBox_Palette.FindString("Default.act");
        }

        //private void GenereteFirstFrame()
        //{
        //    z.Frame f = new z.Frame();
        //    z.Duch0.CopyTo(f.Duch0, 0);
        //    z.Duch1.CopyTo(f.Duch1, 0);
        //    z.Duch2.CopyTo(f.Duch2, 0);
        //    z.Duch3.CopyTo(f.Duch3, 0);
        //    z.Duch0Color.CopyTo(f.Duch0Color, 0);
        //    z.Duch1Color.CopyTo(f.Duch1Color, 0);
        //    z.Duch2Color.CopyTo(f.Duch2Color, 0);
        //    z.Duch3Color.CopyTo(f.Duch3Color, 0);
        //    z.Frames.Add(f);
        //}

        private void ShowFrames()
        {
            if (z.WybranaFrame + z.ZaczynajOdFrame > z.Frames.Count-1)
            {
                if (z.WybranaFrame != 0) z.WybranaFrame--;
                else
                    z.ZaczynajOdFrame--;
            }
            if (z.ZaczynajOdFrame == -1) z.ZaczynajOdFrame = 0;


            label_frame0.Text = "";
            label_frame1.Text = "";
            label_frame2.Text = "";
            label_frame3.Text = "";
            label_frame4.Text = "";
            label_frame5.Text = "";

            pictureBox_frame0.Image = null;
            pictureBox_frame1.Image = null;
            pictureBox_frame2.Image = null;
            pictureBox_frame3.Image = null;
            pictureBox_frame4.Image = null;
            pictureBox_frame5.Image = null;

            ShowFrame(z.ZaczynajOdFrame, pictureBox_frame0, label_frame0);
            if(z.Frames.Count-1 >= z.ZaczynajOdFrame+1)
                ShowFrame(z.ZaczynajOdFrame + 1, pictureBox_frame1, label_frame1);
            if (z.Frames.Count - 1 >= z.ZaczynajOdFrame + 2)
                ShowFrame(z.ZaczynajOdFrame + 2, pictureBox_frame2, label_frame2);
            if (z.Frames.Count - 1 >= z.ZaczynajOdFrame + 3)
                ShowFrame(z.ZaczynajOdFrame + 3, pictureBox_frame3, label_frame3);
            if (z.Frames.Count - 1 >= z.ZaczynajOdFrame + 4)
                ShowFrame(z.ZaczynajOdFrame + 4, pictureBox_frame4, label_frame4);
            if (z.Frames.Count - 1 >= z.ZaczynajOdFrame + 5)
                ShowFrame(z.ZaczynajOdFrame + 5, pictureBox_frame5, label_frame5);
        }
        private void ShowFrame(int indeks, PictureBox pb, Label lb)
        {
            try
            {
                lb.Text = indeks.ToString("d2");
                z.tmpGhostSize = pb.Size;
                int vxsizeGhost = (z.tmpGhostSize.Width / z.PrjGhostSize.Width);
                int vysizeGhost = (z.tmpGhostSize.Height / z.PrjGhostSize.Height);

                Image resultImage = new Bitmap(z.tmpGhostSize.Width, z.tmpGhostSize.Height, PixelFormat.Format24bppRgb);
                using (Graphics grp = Graphics.FromImage(resultImage))
                {
                    // fill background
                    z.atariRGB argb2 = (z.atariRGB)z.palette.GetByIndex(z.KolorTla);
                    SolidBrush sb = new System.Drawing.SolidBrush(argb2.GetColor());
                    grp.FillRectangle(sb, 0, 0, z.tmpGhostSize.Width, z.tmpGhostSize.Height);
                    sb.Dispose();

                    Pen myPen = new Pen(System.Drawing.Color.Gray, 1);
                    z.tmpGhostBlock = new System.Drawing.Size(vxsizeGhost, vysizeGhost);

                    // rysowanie blokow
                    int IndeksDucha = 0;
                    int IndeksKoloru = 0;
                    for (int iy = 0; iy < z.PrjGhostSize.Height; iy++)
                    {
                        //IndeksKoloru = 0;
                        for (int ix = 0; ix < z.PrjGhostSize.Width; ix++)
                        {

                            if (IndeksDucha < z.Duch0.Length)
                            {

                                int znak = 0;
                                int znakold = 0;
                                int color = 0;
                                int colorold = 0;

                                znakold = z.Frames[indeks].Duch3[IndeksDucha];
                                colorold = z.Frames[indeks].Duch3Color[IndeksKoloru];
                                ShowFrame_GenerateBox(grp, iy, ix, znakold, colorold, colorold, false, znakold);
                                znak = z.Frames[indeks].Duch2[IndeksDucha];
                                color = z.Frames[indeks].Duch2Color[IndeksKoloru];
                                ShowFrame_GenerateBox(grp, iy, ix, znak, color, colorold, true, znakold);

                                znakold = z.Frames[indeks].Duch1[IndeksDucha];
                                colorold = z.Frames[indeks].Duch1Color[IndeksKoloru];
                                ShowFrame_GenerateBox(grp, iy, ix, znakold, colorold, colorold, false, znakold);
                                znak = z.Frames[indeks].Duch0[IndeksDucha];
                                color = z.Frames[indeks].Duch0Color[IndeksKoloru];
                                ShowFrame_GenerateBox(grp, iy, ix, znak, color, colorold, true, znakold);
                                IndeksDucha++;
                            }
                            
                        }
                        IndeksKoloru++;
                    }

                    myPen.Dispose();
                }
                pb.Image = resultImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd wykonania funkcji " + FunctionName() + "\r\n\r\n" + ex.ToString());
                //throw new Exception("Błąd wykonania funkcji " + FunctionName() + "\r\n\r\n", ex);
            }

        }
        private static void ShowFrame_GenerateBox(Graphics grp, int iy, int ix, int znak, int color, int colorOld, bool Ora, int znakold)
        {
            if (znak == 1)
            {
                int indeks = Ora && znakold != 0 ? color | colorOld : color;

                z.atariRGB argb = (z.atariRGB)z.palette.GetByIndex(indeks);
                SolidBrush sbb = new System.Drawing.SolidBrush(argb.GetColor());
                grp.FillRectangle(sbb, ix * z.tmpGhostBlock.Width, iy * z.tmpGhostBlock.Height, z.tmpGhostBlock.Width, z.tmpGhostBlock.Height);
                sbb.Dispose();
            }
        }
        private void SetTransparent()
        {
            for (int i = 0; i < z.Duch0.Length; i++)
            {
                z.Duch0[i] = TransparentColor;
                z.Duch1[i] = TransparentColor;
                z.Duch2[i] = TransparentColor;
                z.Duch3[i] = TransparentColor;
            }
        }
        private void ShowGhost()
        {
            try
            {
                z.GhostSize = pictureBox.Size;
                z.GhostMargin = new System.Drawing.Size(32, 32);

                Image resultImage = new Bitmap(z.GhostSize.Width, z.GhostSize.Height, PixelFormat.Format24bppRgb);
                using (Graphics grp = Graphics.FromImage(resultImage))
                {
                    // wyliczenie blokow
                    z.GhostBlock = new System.Drawing.Size((z.GhostSize.Width - z.GhostMargin.Width) / z.PrjGhostSize.Width, (z.GhostSize.Height - z.GhostMargin.Height) / z.PrjGhostSize.Height);

                    // najpier fill controlem, aby ukryc bledy korekty szerokosci i wysokosci
                    //z.atariRGB argb2 = (z.atariRGB)z.palette.GetByIndex(z.KolorTla);
                    SolidBrush sb2 = new System.Drawing.SolidBrush(Color.FromName("control"));
                    grp.FillRectangle(sb2, 0, 0, z.GhostSize.Width, z.GhostSize.Height);

                    // korekta szerokosci i wysokosci 
                    z.GhostSize.Height = z.GhostMargin.Height + (z.GhostBlock.Height * z.PrjGhostSize.Height);
                    z.GhostSize.Width = z.GhostMargin.Width + (z.GhostBlock.Width * z.PrjGhostSize.Width);

                    // fill background
                    z.atariRGB argb2 = (z.atariRGB)z.palette.GetByIndex(z.KolorTla);                    
                    SolidBrush sb = new System.Drawing.SolidBrush(argb2.GetColor());
                    grp.FillRectangle(sb, 0, 0, z.GhostSize.Width, z.GhostSize.Height);
                    sb = new System.Drawing.SolidBrush(Color.FromName("control"));
                    grp.FillRectangle(sb, 0, 0, z.GhostMargin.Width, z.GhostSize.Height);
                    sb.Dispose();
                    
                    // strzalka
                    Bitmap bp = new Bitmap(Properties.Resources.strzalka);
                    grp.DrawImage(bp, new Point(0, 0));

                    // siatka
                    Pen myPen = new Pen(System.Drawing.Color.Gray, 1);
                    

                    // rysowanie blokow

                    if (checkBox_Ghost3.Checked)
                    {
                        int IndeksDucha = 0;
                        for (int iy = 0; iy < z.PrjGhostSize.Height; iy++)
                        {
                            for (int ix = 0; ix < z.PrjGhostSize.Width; ix++)
                            {

                                if (z.Duch3[IndeksDucha] != 0)
                                {
                                    z.atariRGB argb = (z.atariRGB)z.palette.GetByIndex(z.Duch3Color[iy]);
                                    SolidBrush sbb = new System.Drawing.SolidBrush(argb.GetColor());
                                    grp.FillRectangle(sbb, z.GhostMargin.Width + ix * z.GhostBlock.Width, z.GhostMargin.Height + iy * z.GhostBlock.Height, z.GhostBlock.Width, z.GhostBlock.Height);
                                    sbb.Dispose();
                                }
                                IndeksDucha++;
                            }
                        }
                    } 
                    if (checkBox_Ghost2.Checked)
                    {
                        int IndeksDucha = 0;
                        for (int iy = 0; iy < z.PrjGhostSize.Height; iy++)
                        {
                            for (int ix = 0; ix < z.PrjGhostSize.Width; ix++)
                            {

                                if (z.Duch2[IndeksDucha] != 0)
                                {
                                    //int indeks = z.Duch1[IndeksDucha] != 0 && checkBox_Ghost1.Checked ? z.Duch0Color[iy] | z.Duch1Color[iy] : z.Duch0Color[iy];
                                    //int indeks = z.Duch3[IndeksDucha] != 0 && checkBox_Ghost3.Checked ? z.Duch2Color[iy] | z.Duch3Color[iy] : z.Duch2Color[iy];

                                    int indeks = 0;
                                    if (z.Duch3[IndeksDucha] != 0 && checkBox_Ghost3.Checked)
                                    {
                                        indeks = z.Duch2Color[iy] | z.Duch3Color[iy];
                                    }
                                    else
                                        indeks =   z.Duch2Color[iy];
                                    
                                    
                                    //int indeks = z.Duch3[IndeksDucha] != 0 && checkBox_Ghost3.Checked ? 0 : z.Duch2Color[iy];

                                    z.atariRGB argb = (z.atariRGB)z.palette.GetByIndex(indeks);
                                    SolidBrush sbb = new System.Drawing.SolidBrush(argb.GetColor());
                                    grp.FillRectangle(sbb, z.GhostMargin.Width + ix * z.GhostBlock.Width, z.GhostMargin.Height + iy * z.GhostBlock.Height, z.GhostBlock.Width, z.GhostBlock.Height);
                                    sbb.Dispose();
                                }
                                IndeksDucha++;
                            }
                        }
                    }


                    if (checkBox_Ghost1.Checked)
                    {
                        int IndeksDucha = 0;
                        for (int iy = 0; iy < z.PrjGhostSize.Height; iy++)
                        {
                            for (int ix = 0; ix < z.PrjGhostSize.Width; ix++)
                            {

                                if (z.Duch1[IndeksDucha] != 0)
                                {
                                    z.atariRGB argb = (z.atariRGB)z.palette.GetByIndex(z.Duch1Color[iy]);
                                    SolidBrush sbb = new System.Drawing.SolidBrush(argb.GetColor());
                                    grp.FillRectangle(sbb, z.GhostMargin.Width + ix * z.GhostBlock.Width, z.GhostMargin.Height + iy * z.GhostBlock.Height, z.GhostBlock.Width, z.GhostBlock.Height);
                                    sbb.Dispose();
                                }
                                IndeksDucha++;
                            }
                        }
                    } 
                    if (checkBox_Ghost0.Checked)
                    {
                        int IndeksDucha = 0;
                        for (int iy = 0; iy < z.PrjGhostSize.Height; iy++)
                        {
                            for (int ix = 0; ix < z.PrjGhostSize.Width; ix++)
                            {
                                    if (z.Duch0[IndeksDucha] != 0)
                                    {

                                        // ORa kolorów
                                        int indeks = z.Duch1[IndeksDucha] != 0  && checkBox_Ghost1.Checked ? z.Duch0Color[iy] | z.Duch1Color[iy] : z.Duch0Color[iy];
//                                        z.atariRGB argb = (z.atariRGB)z.palette.GetByIndex(z.Duch0Color[iy]);
                                        z.atariRGB argb = (z.atariRGB)z.palette.GetByIndex(indeks);
                                        SolidBrush sbb = new System.Drawing.SolidBrush(argb.GetColor());
                                        grp.FillRectangle(sbb, z.GhostMargin.Width + ix * z.GhostBlock.Width, z.GhostMargin.Height + iy * z.GhostBlock.Height, z.GhostBlock.Width, z.GhostBlock.Height);
                                        sbb.Dispose();
                                    }
                                    IndeksDucha++;
                            }
                        }
                    }






                    
                    // siatka nakładana jako ostatnia
                    for (int ix = 0; ix < z.PrjGhostSize.Width; ix++)
                    {
                        grp.DrawLine(myPen, ix * z.GhostBlock.Width + z.GhostMargin.Width, z.GhostMargin.Height, ix * z.GhostBlock.Width + z.GhostMargin.Width, z.GhostSize.Height);
                    }
                    for (int iy = 0; iy < z.PrjGhostSize.Height; iy++)
                    {
                        grp.DrawLine(myPen, 0, z.GhostMargin.Height + iy * z.GhostBlock.Height, z.GhostSize.Width, z.GhostMargin.Height + iy * z.GhostBlock.Height);
                    }
                    // ostatnie linie dopelnienia
                    grp.DrawLine(myPen, z.GhostSize.Width - 1, 0, z.GhostSize.Width - 1, z.GhostSize.Height);
                    grp.DrawLine(myPen, z.GhostMargin.Width, z.GhostSize.Height - 1, z.GhostSize.Width, z.GhostSize.Height - 1);


//                    grp.DrawLine(myPen,16, 0, 16, z.GhostSize.Height);
                    //grp.DrawLine(myPen, 32, 0, 32, z.GhostSize.Height);
                    //grp.DrawLine(myPen, 48, 0, 48, z.GhostSize.Height);
                    myPen  = new Pen(System.Drawing.Color.GreenYellow, 2);
                    grp.DrawLine(myPen, z.GhostMargin.Width, 0,z.GhostMargin.Width, z.GhostSize.Height);

                    for (int iy = 0; iy < z.PrjGhostSize.Height; iy++)
                    {
                        z.atariRGB argb = (z.atariRGB)z.palette.GetByIndex(z.Duch0Color[0]);
                        int IndeksKoloru = 0;
                        if (z.WybranyLayers == 0) {argb = (z.atariRGB)z.palette.GetByIndex(z.Duch0Color[iy]); IndeksKoloru = z.Duch0Color[iy];}
                        if (z.WybranyLayers == 1) {argb = (z.atariRGB)z.palette.GetByIndex(z.Duch1Color[iy]); IndeksKoloru = z.Duch1Color[iy];}
                        if (z.WybranyLayers == 2) {argb = (z.atariRGB)z.palette.GetByIndex(z.Duch2Color[iy]); IndeksKoloru = z.Duch2Color[iy];}
                        if (z.WybranyLayers == 3) {argb = (z.atariRGB)z.palette.GetByIndex(z.Duch3Color[iy]); IndeksKoloru = z.Duch3Color[iy];}
                        SolidBrush sbb = new System.Drawing.SolidBrush(argb.GetColor());
                        grp.FillRectangle(sbb, 0, z.GhostMargin.Height+ iy * z.GhostBlock.Height, z.GhostMargin.Width, z.GhostBlock.Height);

                        SolidBrush sb33 = new System.Drawing.SolidBrush(InvertMeAColour(argb.GetColor()));
                        Font fnt = SystemFonts.DefaultFont;
                        grp.DrawString("$"+IndeksKoloru.ToString("x2"), fnt, sb33, new System.Drawing.PointF(5, z.GhostMargin.Height+ iy * z.GhostBlock.Height));
                        sb33.Dispose();
                        fnt.Dispose();



                        // ramka na wybranym kolorze
                        if (z.WybranyWierszKoloru == iy)
                        {
                            Color ddd = InvertMeAColour(argb.GetColor());
                            Pen pen = new Pen(ddd, 2);
                            pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset; //<-- this
                            grp.DrawRectangle(pen, 0, z.GhostMargin.Height + iy * z.GhostBlock.Height, z.GhostMargin.Width, z.GhostBlock.Height-1);
                        }

                        sbb.Dispose();
                    }

                    //ramka w trybie wyboru koloru tla
                    if (z.TrybWyboruTla)
                    {
                        z.atariRGB argb = (z.atariRGB)z.palette.GetByIndex(z.KolorTla);
                        Color ddd = InvertMeAColour(argb.GetColor());
                        Pen pen2 = new Pen(ddd, 2);
                        pen2.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset; //<-- this
                        grp.DrawRectangle(pen2, z.GhostMargin.Width, 0, z.GhostSize.Width - z.GhostMargin.Width - 1, z.GhostMargin.Height - 1);
                    }

                    //Brush br = Brushes.Black;
                    //Font fnt = SystemFonts.StatusFont;
                    //grp.DrawString("Kolor", fnt, br, new Point(10, 10));

                    z.atariRGB argb3 = (z.atariRGB)z.palette.GetByIndex(z.KolorTla);                    
                    SolidBrush sb22 = new System.Drawing.SolidBrush(InvertMeAColour(argb3.GetColor()));
                    //Font fnt22 = SystemFonts.DefaultFont;
                    Font fnt22 = new System.Drawing.Font("Arial", 10, FontStyle.Bold);

                    grp.DrawString("BACKGROUND $" + z.KolorTla.ToString("x2"), fnt22, sb22, new System.Drawing.PointF(z.GhostMargin.Width + 200 , 5));
                    sb22.Dispose();
                    fnt22.Dispose();

                    myPen.Dispose();
               }
                pictureBox.Image = resultImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd wykonania funkcji " + FunctionName() + "\r\n\r\n" + ex.ToString());
                //throw new Exception("Błąd wykonania funkcji " + FunctionName() + "\r\n\r\n", ex);
            }
            GenerateGhostsLayers();
            ShowFrames(); 
        }
        //uint InvertColor(uint rgbaColor)
        //{
        //    return 0xFFFFFF00u ^ rgbaColor; // Assumes alpha is in the rightmost byte, change as needed
        //}
        Color InvertMeAColour(Color ColourToInvert)
        {
            return Color.FromArgb((byte)~ColourToInvert.R, (byte)~ColourToInvert.G, (byte)~ColourToInvert.B);
        }
        private void GenerateGhostsLayers()
        {
            GenerateGhostsLayer(pictureBox_Ghost0, 0);
            GenerateGhostsLayer(pictureBox_Ghost1, 1);
            GenerateGhostsLayer(pictureBox_Ghost2, 2);
            GenerateGhostsLayer(pictureBox_Ghost3, 3);
        }
        private void GenerateGhostsLayer(PictureBox pb, int KtoryDuszek)
        {
            try
            {
                z.tmpGhostSize = pb.Size;
                int vxsizeGhost = (z.tmpGhostSize.Width/z.PrjGhostSize.Width) ;
                int vysizeGhost = (z.tmpGhostSize.Height / z.PrjGhostSize.Height);

                Image resultImage = new Bitmap(z.tmpGhostSize.Width, z.tmpGhostSize.Height, PixelFormat.Format24bppRgb);
                using (Graphics grp = Graphics.FromImage(resultImage))
                {
                    // fill background
                    z.atariRGB argb2 = (z.atariRGB)z.palette.GetByIndex(z.KolorTla);
                    SolidBrush sb = new System.Drawing.SolidBrush(argb2.GetColor());
                    grp.FillRectangle(sb, 0, 0, z.tmpGhostSize.Width, z.tmpGhostSize.Height);
                    sb.Dispose();

                    Pen myPen = new Pen(System.Drawing.Color.Gray, 1);
                    z.tmpGhostBlock = new System.Drawing.Size(vxsizeGhost, vysizeGhost);

                    // rysowanie blokow
                    int IndeksDucha = 0;
                    for (int iy = 0; iy < z.PrjGhostSize.Height; iy++)
                    {
                        for (int ix = 0; ix < z.PrjGhostSize.Width; ix++)
                        {
                            
                            if (IndeksDucha < z.Duch0.Length)
                            {

                                int znak = 0;
                                int color = 0;
                                switch (KtoryDuszek)
                                {
                                    case 0:
                                        znak = z.Duch0[IndeksDucha];
                                        color = z.Duch0Color[iy];
                                        break;
                                    case 1:
                                        znak = z.Duch1[IndeksDucha];
                                        color = z.Duch1Color[iy];
                                        break;
                                    case 2:
                                        znak = z.Duch2[IndeksDucha];
                                        color = z.Duch2Color[iy];
                                        break;
                                    case 3:
                                        znak = z.Duch3[IndeksDucha];
                                        color = z.Duch3Color[iy];
                                        break;
                                }
                                
                                if (znak == 1)
                                {
                                    z.atariRGB argb = (z.atariRGB)z.palette.GetByIndex(color);
                                    SolidBrush sbb = new System.Drawing.SolidBrush(argb.GetColor());
                                    grp.FillRectangle(sbb, ix * z.tmpGhostBlock.Width, iy * z.tmpGhostBlock.Height, z.tmpGhostBlock.Width, z.tmpGhostBlock.Height);
                                    sbb.Dispose();
                                }
                                IndeksDucha++;
                            }
                        }
                    }

                    myPen.Dispose();
               }
                pb.Image = resultImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd wykonania funkcji " + FunctionName() + "\r\n\r\n" + ex.ToString());
                //throw new Exception("Błąd wykonania funkcji " + FunctionName() + "\r\n\r\n", ex);
            }

        }
        /// <summary>
        /// zaladuj i wyswiet palete kolorw
        /// </summary>
        /// <param name="FileName"></param>
        //public void ShowPalette(string FileName)
        //{
        //    // load palette from file
        //    // read to bufor
        //    FileStream fs = File.OpenRead(@"palette\" + FileName);
        //    byte[] bytes = new byte[fs.Length];
        //    fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
        //    fs.Close();
        //    // convert to class
        //    for (int i = 0; i < bytes.Length; i += 3)
        //    {
        //        z.palette.Add(i / 3, new z.atariRGB(bytes[i], bytes[i + 1], bytes[i + 2]));
        //    }

        //    z.PaletteSize = pictureBox_palette.Size;
        //    z.PaletteMargin = new System.Drawing.Size(15, 15);
        //    //int xsize = pictureBox_palette.Size.Width;
        //    //int ysize = pictureBox_palette.Size.Height;
        //    int Padding = 2;
        //    Color cBorder = Color.Black;

        //    int xsize = z.PaletteSize.Width - z.PaletteMargin.Width;
        //    int ysize = z.PaletteSize.Height - z.PaletteMargin.Height;

        //    z.PaletteBlock = new System.Drawing.Size(((xsize - Padding) / 8) - (Padding), ((ysize - Padding) / 16) - (Padding));

        //    Image resultImage = new Bitmap(pictureBox_palette.Size.Width, pictureBox_palette.Size.Height, PixelFormat.Format24bppRgb);
        //    using (Graphics grp = Graphics.FromImage(resultImage))
        //    {

        //        SolidBrush sb = new System.Drawing.SolidBrush(Color.FromName("control"));
        //        // fill background
        //        grp.FillRectangle(sb, 0, 0, pictureBox_palette.Size.Width, pictureBox_palette.Size.Height);

        //        Brush br = Brushes.Black;
        //        Font fnt = SystemFonts.StatusFont;
                

        //        int xps = 20;
        //        grp.DrawString("0", fnt, br, xps, 0);
        //        grp.DrawString("2", fnt, br, xps += z.PaletteBlock.Width + Padding, 0);
        //        grp.DrawString("4", fnt, br, xps += z.PaletteBlock.Width + Padding, 0);
        //        grp.DrawString("6", fnt, br, xps += z.PaletteBlock.Width + Padding, 0);
        //        grp.DrawString("8", fnt, br, xps += z.PaletteBlock.Width + Padding, 0);
        //        grp.DrawString("A", fnt, br, xps += z.PaletteBlock.Width + Padding, 0);
        //        grp.DrawString("C", fnt, br, xps += z.PaletteBlock.Width + Padding, 0);
        //        grp.DrawString("E", fnt, br, xps += z.PaletteBlock.Width + Padding, 0);


        //        int yps = 17;
        //        grp.DrawString("0", fnt, br, 2, yps);
        //        grp.DrawString("1", fnt, br, 2, yps += z.PaletteBlock.Height + Padding);
        //        grp.DrawString("2", fnt, br, 2, yps += z.PaletteBlock.Height + Padding);
        //        grp.DrawString("3", fnt, br, 2, yps += z.PaletteBlock.Height + Padding);
        //        grp.DrawString("4", fnt, br, 2, yps += z.PaletteBlock.Height + Padding);
        //        grp.DrawString("5", fnt, br, 2, yps += z.PaletteBlock.Height + Padding);
        //        grp.DrawString("6", fnt, br, 2, yps += z.PaletteBlock.Height + Padding);
        //        grp.DrawString("7", fnt, br, 2, yps += z.PaletteBlock.Height + Padding);
        //        grp.DrawString("8", fnt, br, 2, yps += z.PaletteBlock.Height + Padding);
        //        grp.DrawString("9", fnt, br, 2, yps += z.PaletteBlock.Height + Padding);
        //        grp.DrawString("A", fnt, br, 2, yps += z.PaletteBlock.Height + Padding);
        //        grp.DrawString("B", fnt, br, 2, yps += z.PaletteBlock.Height + Padding);
        //        grp.DrawString("C", fnt, br, 2, yps += z.PaletteBlock.Height + Padding);
        //        grp.DrawString("D", fnt, br, 2, yps += z.PaletteBlock.Height + Padding);
        //        grp.DrawString("E", fnt, br, 2, yps += z.PaletteBlock.Height + Padding);
        //        grp.DrawString("F", fnt, br, 2, yps += z.PaletteBlock.Height + Padding);

        //        br.Dispose();


        //        // color 8x16
        //        int IndeksColor = 0;
        //        for (int iy = 0; iy < 16; iy++)
        //        {
        //            for (int ix = 0; ix < 8; ix++)
        //            {
        //                z.atariRGB argb = (z.atariRGB)z.palette.GetByIndex(IndeksColor);
        //                System.Drawing.SolidBrush myBrush;
        //                myBrush = new System.Drawing.SolidBrush(argb.GetColor());

        //                int xp = 15 + (ix * z.PaletteBlock.Width + (ix + 1) * Padding);
        //                int yp = 15 + (iy * z.PaletteBlock.Height + (iy + 1) * Padding);

        //                grp.FillRectangle(myBrush, xp, yp, z.PaletteBlock.Width, z.PaletteBlock.Height);

        //                Pen pen = new Pen(cBorder, 1);
        //                pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset; //<-- this
        //                grp.DrawRectangle(pen, xp, yp, z.PaletteBlock.Width, z.PaletteBlock.Height);

        //                IndeksColor++;
        //                myBrush.Dispose();
        //            }
        //        }

        //    }
        //    pictureBox_palette.Image = resultImage;
        //}
        public void ShowPalette(string FileName)
        {
            // load palette from file
            // read to bufor
            FileStream fs = File.OpenRead(@"palette\" + FileName);
            byte[] bytes = new byte[fs.Length];
            fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
            fs.Close();
            // convert to class
            z.palette.Clear();
            for (int i = 0; i < bytes.Length; i += 3)
            {
                z.palette.Add(i / 3, new z.atariRGB(bytes[i], bytes[i + 1], bytes[i + 2]));
            }

            z.PaletteSize = pictureBox_palette.Size;
            z.PaletteMargin = new System.Drawing.Size(0, 0);
            //int xsize = pictureBox_palette.Size.Width;
            //int ysize = pictureBox_palette.Size.Height;
            int Padding = 2;
            Color cBorder = Color.Black;

            int xsize = z.PaletteSize.Width - z.PaletteMargin.Width;
            int ysize = z.PaletteSize.Height - z.PaletteMargin.Height;

            z.PaletteBlock = new System.Drawing.Size(((xsize - Padding) / 8) - (Padding), ((ysize - Padding) / 16) - (Padding));

            Image resultImage = new Bitmap(pictureBox_palette.Size.Width, pictureBox_palette.Size.Height, PixelFormat.Format24bppRgb);
            using (Graphics grp = Graphics.FromImage(resultImage))
            {

                SolidBrush sb = new System.Drawing.SolidBrush(Color.FromName("control"));
                // fill background
                grp.FillRectangle(sb, 0, 0, pictureBox_palette.Size.Width, pictureBox_palette.Size.Height);

                Brush br = Brushes.Black;
                Font fnt = SystemFonts.DefaultFont;
                Font fnt2 = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
                
                

                //int xps = 20;
                //grp.DrawString("0", fnt, br, xps, 0);
                //grp.DrawString("2", fnt, br, xps += z.PaletteBlock.Width + Padding, 0);
                //grp.DrawString("4", fnt, br, xps += z.PaletteBlock.Width + Padding, 0);
                //grp.DrawString("6", fnt, br, xps += z.PaletteBlock.Width + Padding, 0);
                //grp.DrawString("8", fnt, br, xps += z.PaletteBlock.Width + Padding, 0);
                //grp.DrawString("A", fnt, br, xps += z.PaletteBlock.Width + Padding, 0);
                //grp.DrawString("C", fnt, br, xps += z.PaletteBlock.Width + Padding, 0);
                //grp.DrawString("E", fnt, br, xps += z.PaletteBlock.Width + Padding, 0);

                //int yps = 17;
                //grp.DrawString("0", fnt, br, 2, yps);
                //grp.DrawString("1", fnt, br, 2, yps += z.PaletteBlock.Height + Padding);
                //grp.DrawString("2", fnt, br, 2, yps += z.PaletteBlock.Height + Padding);
                //grp.DrawString("3", fnt, br, 2, yps += z.PaletteBlock.Height + Padding);
                //grp.DrawString("4", fnt, br, 2, yps += z.PaletteBlock.Height + Padding);
                //grp.DrawString("5", fnt, br, 2, yps += z.PaletteBlock.Height + Padding);
                //grp.DrawString("6", fnt, br, 2, yps += z.PaletteBlock.Height + Padding);
                //grp.DrawString("7", fnt, br, 2, yps += z.PaletteBlock.Height + Padding);
                //grp.DrawString("8", fnt, br, 2, yps += z.PaletteBlock.Height + Padding);
                //grp.DrawString("9", fnt, br, 2, yps += z.PaletteBlock.Height + Padding);
                //grp.DrawString("A", fnt, br, 2, yps += z.PaletteBlock.Height + Padding);
                //grp.DrawString("B", fnt, br, 2, yps += z.PaletteBlock.Height + Padding);
                //grp.DrawString("C", fnt, br, 2, yps += z.PaletteBlock.Height + Padding);
                //grp.DrawString("D", fnt, br, 2, yps += z.PaletteBlock.Height + Padding);
                //grp.DrawString("E", fnt, br, 2, yps += z.PaletteBlock.Height + Padding);
                //grp.DrawString("F", fnt, br, 2, yps += z.PaletteBlock.Height + Padding);

                


                // color 8x16
                int IndeksColor = 0;
                for (int iy = 0; iy < 16; iy++)
                {
                    for (int ix = 0; ix < 8; ix++)
                    {
                        z.atariRGB argb = (z.atariRGB)z.palette.GetByIndex(IndeksColor);
                        System.Drawing.SolidBrush myBrush;
                        myBrush = new System.Drawing.SolidBrush(argb.GetColor());

                        int xp = (ix * z.PaletteBlock.Width + (ix + 1) * Padding);
                        int yp = (iy * z.PaletteBlock.Height + (iy + 1) * Padding);

                        grp.FillRectangle(myBrush, xp, yp, z.PaletteBlock.Width, z.PaletteBlock.Height);
                        if (checkBox_ShowIndex.Checked)
                        {
                            SolidBrush sb33 = new System.Drawing.SolidBrush(InvertMeAColour(argb.GetColor()));
                            grp.DrawString("$"+IndeksColor.ToString("x2"), fnt2, sb33, xp+6, yp);
                            sb33.Dispose();
                        }

                        Pen pen = new Pen(cBorder, 1);
                        pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset; //<-- this
                        grp.DrawRectangle(pen, xp, yp, z.PaletteBlock.Width, z.PaletteBlock.Height);

                        IndeksColor = IndeksColor + 2;
                        myBrush.Dispose();
                    }
                }


                br.Dispose();

            }
            pictureBox_palette.Image = resultImage;
        }
        public string Version(string name, int BigVersion)
        {
            // data kompilacji
            string filePath = System.Reflection.Assembly.GetCallingAssembly().Location;
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;
            byte[] b = new byte[2048];
            System.IO.Stream s = null;

            try
            {
                s = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                s.Read(b, 0, 2048);
            }
            finally
            {
                if (s != null)
                {
                    s.Close();
                }
            }

            int i = System.BitConverter.ToInt32(b, c_PeHeaderOffset);
            int secondsSince1970 = System.BitConverter.ToInt32(b, i + c_LinkerTimestampOffset);
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);
            dt = dt.AddSeconds(secondsSince1970);
            dt = dt.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours);
            return name + " v" + BigVersion + "." + dt.ToString("yyMMdd");
        }
        /// <summary>
        /// Zapamietuje nazwe wywołanej funkcji - do zgłaszania błędów
        /// </summary>
        public string FunctionName()
        {
            string sWynik = "";
            try
            {
                System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(1);
                System.Diagnostics.StackFrame sf = st.GetFrame(0);
                sWynik = sf.GetMethod().DeclaringType.FullName + "." + sf.GetMethod().Name;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return sWynik;
        }
        private void pictureBox_Click(object sender, EventArgs e)
        {
            
        }
        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            Point pozycja = e.Location;
            // przelicz ktory to klocek
            if (z.GhostMargin.Width <= pozycja.X && z.GhostMargin.Height <= pozycja.Y)
            {
                int px = (pozycja.X - z.GhostMargin.Width) / z.GhostBlock.Width;
                int py = (pozycja.Y - z.GhostMargin.Height) / z.GhostBlock.Height;

                if (z.WybranyLayers == 0 && !checkBox_GhostSecure0.Checked)
                    z.Duch0[py * 8 + px] = z.Duch0[py * 8 + px] == 1 ? (byte)0 : (byte)1;
                if (z.WybranyLayers == 1 && !checkBox_GhostSecure1.Checked)
                    z.Duch1[py * 8 + px] = z.Duch1[py * 8 + px] == 1 ? (byte)0 : (byte)1;
                if (z.WybranyLayers == 2 && !checkBox_GhostSecure2.Checked)
                    z.Duch2[py * 8 + px] = z.Duch2[py * 8 + px] == 1 ? (byte)0 : (byte)1;
                if (z.WybranyLayers == 3 && !checkBox_GhostSecure3.Checked)
                    z.Duch3[py * 8 + px] = z.Duch3[py * 8 + px] == 1 ? (byte)0 : (byte)1;
                //GenerateGhostsLayers();
            }
            // a moze to kolor?
            if (z.GhostMargin.Width >= pozycja.X && z.GhostMargin.Height <= pozycja.Y)
            {
                if (
                    (z.WybranyLayers == 0 && !checkBox_GhostSecure0.Checked)
                    || (z.WybranyLayers == 1 && !checkBox_GhostSecure1.Checked)
                    || (z.WybranyLayers == 2 && !checkBox_GhostSecure2.Checked)
                    || (z.WybranyLayers == 3 && !checkBox_GhostSecure3.Checked))
                {
                    int px = (pozycja.X) / z.GhostBlock.Width;
                    int py = (pozycja.Y - z.GhostMargin.Height) / z.GhostBlock.Height;
                    z.WybranyWierszKoloru = (byte)py;
                }
            }

            // rozpisac kolor?   Strzałeczka w dół
            if (z.GhostMargin.Width >= pozycja.X && z.GhostMargin.Height >= pozycja.Y)
            {
                int px = (pozycja.X) / z.GhostBlock.Width;
                int py = (pozycja.Y - z.GhostMargin.Height) / z.GhostBlock.Height;
                byte color = 0;
                switch (z.WybranyLayers)
                {
                    case 0:
                        color = z.Duch0Color[z.WybranyWierszKoloru];
                        break;
                    case 1:
                        color = z.Duch1Color[z.WybranyWierszKoloru];
                        break;
                    case 2:
                        color = z.Duch2Color[z.WybranyWierszKoloru];
                        break;
                    case 3:
                        color = z.Duch3Color[z.WybranyWierszKoloru];
                        break;
                }
                for (int i = z.WybranyWierszKoloru; i < 256; i++)
                {
                    switch (z.WybranyLayers)
                    {
                        case 0:
                            z.Duch0Color[i] = color;
                            break;
                        case 1:
                            z.Duch1Color[i] = color;
                            break;
                        case 2:
                            z.Duch2Color[i] = color;
                            break;
                        case 3:
                            z.Duch3Color[i] = color;
                            break;
                    }
                }
                
            }


            // a moze to kolor tla?
            if (z.GhostMargin.Width <= pozycja.X && z.GhostMargin.Height >= pozycja.Y)
            {
                int px = (pozycja.X) / z.GhostBlock.Width;
                int py = (pozycja.Y - z.GhostMargin.Height) / z.GhostBlock.Height;
                z.TrybWyboruTla = !z.TrybWyboruTla;
                
            }
            CopyGhostToFrame();
            ShowGhost();
        }
        private void tableLayout_Layers_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            Color cr = Color.FromName("blue");
            SolidBrush sb = new System.Drawing.SolidBrush(cr);

            if (e.Column == z.WybranyLayers+1)
                e.Graphics.FillRectangle(sb, e.CellBounds);

            
            checkBox_Ghost0.BackColor = Color.FromName("control");
            checkBox_Ghost1.BackColor = Color.FromName("control");
            checkBox_Ghost2.BackColor = Color.FromName("control");
            checkBox_Ghost3.BackColor = Color.FromName("control");
            checkBox_GhostSecure0.BackColor = Color.FromName("control");
            checkBox_GhostSecure1.BackColor = Color.FromName("control");
            checkBox_GhostSecure2.BackColor = Color.FromName("control");
            checkBox_GhostSecure3.BackColor = Color.FromName("control");
            label_Ghost0.BackColor = Color.FromName("control");
            label_Ghost1.BackColor = Color.FromName("control");
            label_Ghost2.BackColor = Color.FromName("control");
            label_Ghost3.BackColor = Color.FromName("control");

            if (z.WybranyLayers == 0)
                label_Ghost0.BackColor = checkBox_GhostSecure0.BackColor = checkBox_Ghost0.BackColor = cr;
            if (z.WybranyLayers == 1)
                label_Ghost1.BackColor = checkBox_GhostSecure1.BackColor = checkBox_Ghost1.BackColor = cr;
            if (z.WybranyLayers == 2)
                label_Ghost2.BackColor = checkBox_GhostSecure2.BackColor = checkBox_Ghost2.BackColor = cr;
            if (z.WybranyLayers == 3)
                label_Ghost3.BackColor = checkBox_GhostSecure3.BackColor = checkBox_Ghost3.BackColor = cr;
        }
        private void pictureBox_Ghost0_Click(object sender, EventArgs e)
        {
            z.WybranyLayers = 0;
            ShowGhost();
            tableLayout_Layers.Refresh();
        }
        private void pictureBox_Ghost1_Click(object sender, EventArgs e)
        {
            z.WybranyLayers = 1;
            ShowGhost();
            tableLayout_Layers.Refresh();
        }
        private void pictureBox_Ghost2_Click(object sender, EventArgs e)
        {
            z.WybranyLayers = 2;
            ShowGhost();
            tableLayout_Layers.Refresh();
        }
        private void pictureBox_Ghost3_Click(object sender, EventArgs e)
        {
            z.WybranyLayers = 3;
            ShowGhost();
            tableLayout_Layers.Refresh();
        }
        private void SaveData(string FileName)
        {
            FileStream fw = File.OpenWrite(FileName);
            fw.WriteByte(Convert.ToByte('d'));
            fw.WriteByte(Convert.ToByte('c'));
            fw.WriteByte(Convert.ToByte('e'));
            fw.WriteByte((byte)z.PrjGhostSize.Width);
            fw.WriteByte((byte)z.PrjGhostSize.Height);
            fw.WriteByte(z.KolorTla);
            fw.Write(z.Duch0, 0, z.Duch0.Length);
            fw.Write(z.Duch1, 0, z.Duch1.Length);
            fw.Write(z.Duch2, 0, z.Duch2.Length);
            fw.Write(z.Duch3, 0, z.Duch3.Length);
            fw.Write(z.Duch0Color, 0, z.Duch0Color.Length);
            fw.Write(z.Duch1Color, 0, z.Duch1Color.Length);
            fw.Write(z.Duch2Color, 0, z.Duch2Color.Length);
            fw.Write(z.Duch3Color, 0, z.Duch3Color.Length);
            fw.WriteByte((byte)z.Frames.Count);
            foreach (z.Frame item in z.Frames)
            {
                fw.Write(item.Duch0, 0, item.Duch0.Length);
                fw.Write(item.Duch1, 0, item.Duch1.Length);
                fw.Write(item.Duch2, 0, item.Duch2.Length);
                fw.Write(item.Duch3, 0, item.Duch3.Length);
                fw.Write(item.Duch0Color, 0, item.Duch0Color.Length);
                fw.Write(item.Duch1Color, 0, item.Duch1Color.Length);
                fw.Write(item.Duch2Color, 0, item.Duch2Color.Length);
                fw.Write(item.Duch3Color, 0, item.Duch3Color.Length);
            }
            fw.Close();
        }
        private void LoadData(string FileName)
        {
            FileStream fs = File.OpenRead(FileName);
            byte[] bajt = new byte[6];
            fs.Read(bajt, 0, 6);
            z.PrjGhostSize = new System.Drawing.Size((int)bajt[3], (int)bajt[4]);
            z.KolorTla = bajt[5];
            fs.Read(z.Duch0, 0, Convert.ToInt32(z.Duch0.Length));
            fs.Read(z.Duch1, 0, Convert.ToInt32(z.Duch1.Length));
            fs.Read(z.Duch2, 0, Convert.ToInt32(z.Duch2.Length));
            fs.Read(z.Duch3, 0, Convert.ToInt32(z.Duch3.Length));
            fs.Read(z.Duch0Color, 0, Convert.ToInt32(z.Duch0Color.Length));
            fs.Read(z.Duch1Color, 0, Convert.ToInt32(z.Duch1Color.Length));
            fs.Read(z.Duch2Color, 0, Convert.ToInt32(z.Duch2Color.Length));
            fs.Read(z.Duch3Color, 0, Convert.ToInt32(z.Duch3Color.Length));
            z.Frames.Clear();
            byte[] IleFrames = new byte[1];
            fs.Read(IleFrames, 0, 1);
            for (int i = 0; i < IleFrames[0]; i++)
            {
                z.Frame f = new z.Frame();
                fs.Read(f.Duch0, 0, Convert.ToInt32(f.Duch0.Length));
                fs.Read(f.Duch1, 0, Convert.ToInt32(f.Duch1.Length));
                fs.Read(f.Duch2, 0, Convert.ToInt32(f.Duch2.Length));
                fs.Read(f.Duch3, 0, Convert.ToInt32(f.Duch3.Length));
                fs.Read(f.Duch0Color, 0, Convert.ToInt32(f.Duch0Color.Length));
                fs.Read(f.Duch1Color, 0, Convert.ToInt32(f.Duch1Color.Length));
                fs.Read(f.Duch2Color, 0, Convert.ToInt32(f.Duch2Color.Length));
                fs.Read(f.Duch3Color, 0, Convert.ToInt32(f.Duch3Color.Length));
                z.Frames.Add(f);
            }
            fs.Close();

            // spr czy wszystko zaladowal
            if (z.Frames.Count == 0)
            {
                z.PrjGhostSize = new System.Drawing.Size(8, 18);
                button_AddFrame_Click(null, null);
            }
            SetHeight();
            FramesPrzeliczScroll();
            SprMoveButtonOnOff();
        }
        private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //SaveData("dane.dce");
        }
        private void checkBox_Ghost0_CheckedChanged(object sender, EventArgs e)
        {
            ShowGhost();
        }
        private void checkBox_Ghost1_CheckedChanged(object sender, EventArgs e)
        {
            ShowGhost();
        }
        private void checkBox_Ghost2_CheckedChanged(object sender, EventArgs e)
        {
            ShowGhost();
        }
        private void checkBox_Ghost3_CheckedChanged(object sender, EventArgs e)
        {
            ShowGhost();
        }
        private void pictureBox_palette_MouseDown(object sender, MouseEventArgs e)
        {
            Point pozycja = e.Location;
            // przelicz ktory to klocek
            if (z.PaletteMargin.Width <= pozycja.X && z.PaletteMargin.Height <= pozycja.Y)
            {
                int px = (pozycja.X - z.PaletteMargin.Width) / (z.PaletteBlock.Width + 2);
                int py = (pozycja.Y - z.PaletteMargin.Height) / (z.PaletteBlock.Height + 2);
                int IndeksKoloru = py * 16 + px *2 ;
                Console.WriteLine(px.ToString("x2") + " " + py.ToString("x2") + " " + IndeksKoloru.ToString("x2"));

                if (z.TrybWyboruTla)
                {
                    z.KolorTla = (byte)IndeksKoloru;
                    z.TrybWyboruTla = !z.TrybWyboruTla;
                }
                else
                {
                    if (z.WybranyLayers == 0)
                        z.Duch0Color[z.WybranyWierszKoloru] = (byte)IndeksKoloru;
                    if (z.WybranyLayers == 1)
                        z.Duch1Color[z.WybranyWierszKoloru] = (byte)IndeksKoloru;
                    if (z.WybranyLayers == 2)
                        z.Duch2Color[z.WybranyWierszKoloru] = (byte)IndeksKoloru;
                    if (z.WybranyLayers == 3)
                        z.Duch3Color[z.WybranyWierszKoloru] = (byte)IndeksKoloru;
                }

                CopyGhostToFrame();
                ShowGhost();
                //GenerateGhostsLayers();
            }
        }
        private void pictureBox_Oko_Click(object sender, EventArgs e)
        {
            if (z.WidocznyLayers == 255)
            {
                z.WidocznyLayers = z.WybranyLayers;
                checkBox_Ghost0.Checked = z.WidocznyLayers == 0 ? true : false;
                checkBox_Ghost1.Checked = z.WidocznyLayers == 1 ? true : false;
                checkBox_Ghost2.Checked = z.WidocznyLayers == 2 ? true : false;
                checkBox_Ghost3.Checked = z.WidocznyLayers == 3 ? true : false;
            }
            else
            {
                z.WidocznyLayers = 255;
                checkBox_Ghost0.Checked = true;
                checkBox_Ghost1.Checked = true;
                checkBox_Ghost2.Checked = true;
                checkBox_Ghost3.Checked = true;
            }
            ShowGhost();
            //GenerateGhostsLayers();
        }
        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

        }
        private void SetHeight()
        {
            numericUpDown_Height.Value = z.PrjGhostSize.Height;
        }
        private void numericUpDown_Height_ValueChanged(object sender, EventArgs e)
        {
            z.PrjGhostSize.Height = (int)numericUpDown_Height.Value;
            ShowGhost();
            //GenerateGhostsLayers();
        }
        private void button_Load_Click(object sender, EventArgs e)
        {

            DialogResult dialogResult = MessageBox.Show("You will lose the current data! Are you sure?", "Caution!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    //OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "Dan Casper Editor|*.dce";
                    openFileDialog.Title = "Plik projektu";
                    if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        LoadData(openFileDialog.FileName);
                    }
                    z.WybranaFrame = 0;
                    z.WybranyLayers = 0;
                    z.WybranyWierszKoloru = 0;
                    CopyFrameToGhost();
                    ShowGhost();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd wykonania funkcji " + FunctionName() + "\r\n\r\n" + ex.ToString());
                    //throw new Exception("Błąd wykonania funkcji " + FunctionName() + "\r\n\r\n", ex);
                }
            }
        }
        private void button_Save_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "Dan Casper Editor|*.dce";
            saveFileDialog.Title = "Plik projektu";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SaveData(saveFileDialog.FileName);
            }
        }
        private void button_Export_Click(object sender, EventArgs e)
        {
            contextMenuStrip_Export.Show(Cursor.Position);
            
            
            //saveFileDialog.Filter = "Binary Dan Casper Editor|*.bin";
            //saveFileDialog.Title = "Plik eksportu";
            //if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    ExportBins(saveFileDialog.FileName);
            //}
        }
        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("You will lose the current data! Are you sure?", "Caution!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                ClearGhost();
                z.Frames.Clear();
                z.Frames.Add(new z.Frame());
                z.WybranaFrame = 0;
                z.ZaczynajOdFrame = 0;
                ShowGhost();
                SprMoveButtonOnOff();
            }
        }
        private static void ClearGhost()
        {
            for (int i = 0; i < z.Duch0.Length; i++)
            {
                z.Duch0[i] = 0;
                z.Duch1[i] = 0;
                z.Duch2[i] = 0;
                z.Duch3[i] = 0;
            }
            for (int i = 0; i < z.Duch0Color.Length; i++)
            {
                z.Duch0Color[i] = 0;
                z.Duch1Color[i] = 0;
                z.Duch2Color[i] = 0;
                z.Duch3Color[i] = 0;
            }
        }
        private void button_AddFrame_Click(object sender, EventArgs e)
        {
            if (z.Frames.Count < 255)
            {
                z.Frame f = new z.Frame();
                z.Duch0.CopyTo(f.Duch0, 0);
                z.Duch1.CopyTo(f.Duch1, 0);
                z.Duch2.CopyTo(f.Duch2, 0);
                z.Duch3.CopyTo(f.Duch3, 0);
                z.Duch0Color.CopyTo(f.Duch0Color, 0);
                z.Duch1Color.CopyTo(f.Duch1Color, 0);
                z.Duch2Color.CopyTo(f.Duch2Color, 0);
                z.Duch3Color.CopyTo(f.Duch3Color, 0);
                //z.Frames.Add(f);
                z.Frames.Insert(z.WybranaFrame+z.ZaczynajOdFrame, f);
                z.WybranaFrame++;
                if(z.WybranaFrame>5)
                {
                    z.WybranaFrame = 5;
                    z.ZaczynajOdFrame++;
                }
                ShowFrames();
            }
            FramesPrzeliczScroll();
            SprMoveButtonOnOff();
        }
        private void button_DelFrame_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete the current frame of animation?", "Caution!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {

                if (z.Frames.Count != 1)
                {
                    z.Frames.RemoveAt(z.WybranaFrame + z.ZaczynajOdFrame);
                }

                if ((z.Frames.Count - 1) < z.WybranaFrame + z.ZaczynajOdFrame)
                {
                    z.ZaczynajOdFrame--;
                    if (z.ZaczynajOdFrame < 0)
                    {
                        z.ZaczynajOdFrame = 0;
                        z.WybranaFrame--;
                        if (z.WybranaFrame < 0)
                            z.WybranaFrame = 0;
                    }
                }
                //z.WybranaFrame--;
                //if (z.WybranaFrame < 0)
                //{
                //    z.WybranaFrame = 0;
                //    z.ZaczynajOdFrame--;
                //    if (z.ZaczynajOdFrame < 0) z.ZaczynajOdFrame = 0;
                //}
                CopyFrameToGhost();
                ShowGhost();
                FramesPrzeliczScroll();
                SprMoveButtonOnOff();
            }
        }
        private void FramesPrzeliczScroll()
        {
            hScrollBar.Maximum = z.Frames.Count - 6 < 0 ? 0 : z.Frames.Count - 6;
            hScrollBar.Value = z.ZaczynajOdFrame;
        }
        private void tableLayoutPanel4_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            Color cr = Color.FromName("blue");
            SolidBrush sb = new System.Drawing.SolidBrush(cr);

            if (e.Column == z.WybranaFrame)
                e.Graphics.FillRectangle(sb, e.CellBounds);

            label_frame0.BackColor = Color.FromName("control");
            label_frame1.BackColor = Color.FromName("control");
            label_frame2.BackColor = Color.FromName("control");
            label_frame3.BackColor = Color.FromName("control");
            label_frame4.BackColor = Color.FromName("control");
            label_frame5.BackColor = Color.FromName("control");

            if (z.WybranaFrame  == 0) label_frame0.BackColor = cr;
            if (z.WybranaFrame == 1) label_frame1.BackColor = cr;
            if (z.WybranaFrame == 2) label_frame2.BackColor = cr;
            if (z.WybranaFrame == 3) label_frame3.BackColor = cr;
            if (z.WybranaFrame  == 4) label_frame4.BackColor = cr;
            if (z.WybranaFrame == 5) label_frame5.BackColor = cr;

            label_frame0.Refresh();
            label_frame1.Refresh();
            label_frame2.Refresh();
            label_frame3.Refresh();
            label_frame4.Refresh();
            label_frame5.Refresh();
        }
        private void SelectFrame()
        {
            PrzeliczKlatki();
            //CopyFrameToGhost();
            ShowGhost();
            tableLayoutPanel_FramesTools.Refresh();

        }
        private void PrzeliczKlatki()
        {
            int odKlatki = z.ZaczynajOdFrame + z.WybranaFrame;
            if (odKlatki > z.Frames.Count - 1)
                z.WybranaFrame = (z.Frames.Count - 1 - z.ZaczynajOdFrame);
            SprMoveButtonOnOff();
        }
        private void pictureBox_frame0_Click(object sender, EventArgs e)
        {
            z.WybranaFrame = 0;
            PrzeliczKlatki();
            CopyFrameToGhost(); 
            SelectFrame();
        }
        private void pictureBox_frame1_Click(object sender, EventArgs e)
        {
            z.WybranaFrame = 1;
            PrzeliczKlatki();
            
            CopyFrameToGhost();
            SelectFrame();
        }
        private void pictureBox_frame2_Click(object sender, EventArgs e)
        {
            z.WybranaFrame = 2;
            PrzeliczKlatki();
            CopyFrameToGhost();
            SelectFrame();
        }
        private void pictureBox_frame3_Click(object sender, EventArgs e)
        {
            z.WybranaFrame = 3;
            PrzeliczKlatki();
            CopyFrameToGhost();
            SelectFrame();
        }
        private void pictureBox_frame4_Click(object sender, EventArgs e)
        {
            z.WybranaFrame = 4;
            PrzeliczKlatki();
            CopyFrameToGhost();
            SelectFrame();
        }
        private void pictureBox_frame5_Click(object sender, EventArgs e)
        {
            z.WybranaFrame = 5;
            PrzeliczKlatki();
            CopyFrameToGhost();
            SelectFrame();
        }
        private void CopyFrames(int Source, int Destination)
        {
            z.Frame fs = z.Frames[Source];
            z.Frame fd = z.Frames[Destination];
            z.Frames[Source] = fd;
            z.Frames[Destination] = fs;
        }
        private void button_RightFrame_Click(object sender, EventArgs e)
        {
            
            //CopyFrameToGhost();
            //ShowGhost();
            //tableLayoutPanel_FramesTools.Refresh();
            CopyFrames(z.WybranaFrame + z.ZaczynajOdFrame, z.WybranaFrame + z.ZaczynajOdFrame + 1);
            z.WybranaFrame++;
            if(z.WybranaFrame>5) 
            {
                z.WybranaFrame = 5;
                z.ZaczynajOdFrame++;
                if (z.ZaczynajOdFrame > z.Frames.Count-1)
                    z.ZaczynajOdFrame = z.Frames.Count - 1;
            }
            ShowGhost();
            hScrollBar.Value = z.ZaczynajOdFrame;
            SprMoveButtonOnOff();
        }
        private void button_LeftFrame_Click(object sender, EventArgs e)
        {
            
            //CopyFrameToGhost();
            //ShowGhost(); 
            //tableLayoutPanel_FramesTools.Refresh();
            CopyFrames(z.WybranaFrame + z.ZaczynajOdFrame, z.WybranaFrame + z.ZaczynajOdFrame - 1);

            z.WybranaFrame--;
            if (z.WybranaFrame < 0)
            {
                z.WybranaFrame = 0;
                z.ZaczynajOdFrame--;
                if (z.ZaczynajOdFrame < 0)
                    z.ZaczynajOdFrame = 0;
            }

            ShowGhost();
//            z.ZaczynajOdFrame = z.ZaczynajOdFrame - 1 < 0 ? 0 : z.ZaczynajOdFrame - 1;
            hScrollBar.Value = z.ZaczynajOdFrame;
            SprMoveButtonOnOff();
        }
        private void CopyGhostToFrame()
        {

            int iFrame = z.WybranaFrame + z.ZaczynajOdFrame;
            Console.WriteLine("ToFrame " + iFrame);
            z.Frame f = z.Frames[iFrame];
            z.Duch0.CopyTo(f.Duch0, 0);
            z.Duch1.CopyTo(f.Duch1, 0);
            z.Duch2.CopyTo(f.Duch2, 0);
            z.Duch3.CopyTo(f.Duch3, 0);
            z.Duch0Color.CopyTo(f.Duch0Color, 0);
            z.Duch1Color.CopyTo(f.Duch1Color, 0);
            z.Duch2Color.CopyTo(f.Duch2Color, 0);
            z.Duch3Color.CopyTo(f.Duch3Color, 0);
            z.Frames[iFrame] = f;
        }
        private void CopyFrameToGhost()
        {
            int iFrame = z.WybranaFrame + z.ZaczynajOdFrame;
            Console.WriteLine("ToGhost " + iFrame);
            z.Frame f = z.Frames[iFrame];
            f.Duch0.CopyTo(z.Duch0, 0);
            f.Duch1.CopyTo(z.Duch1, 0);
            f.Duch2.CopyTo(z.Duch2, 0);
            f.Duch3.CopyTo(z.Duch3, 0);
            f.Duch0Color.CopyTo(z.Duch0Color, 0);
            f.Duch1Color.CopyTo(z.Duch1Color, 0);
            f.Duch2Color.CopyTo(z.Duch2Color, 0);
            f.Duch3Color.CopyTo(z.Duch3Color, 0);
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                PictureBox pb = pictureBox_Animation;

                z.AnimationFrame++;
                if (z.AnimationFrame > z.Frames.Count-1 )
                    z.AnimationFrame = 0;


                z.tmpGhostSize = pb.Size;
                int vxsizeGhost = (z.tmpGhostSize.Width / z.PrjGhostSize.Width);
                int vysizeGhost = (z.tmpGhostSize.Height / z.PrjGhostSize.Height);

                Image resultImage = new Bitmap(z.tmpGhostSize.Width, z.tmpGhostSize.Height, PixelFormat.Format24bppRgb);
                using (Graphics grp = Graphics.FromImage(resultImage))
                {
                    // fill background
                    z.atariRGB argb2 = (z.atariRGB)z.palette.GetByIndex(z.KolorTla);
                    SolidBrush sb = new System.Drawing.SolidBrush(argb2.GetColor());
                    grp.FillRectangle(sb, 0, 0, z.tmpGhostSize.Width, z.tmpGhostSize.Height);
                    sb.Dispose();

                    Pen myPen = new Pen(System.Drawing.Color.Gray, 1);
                    z.tmpGhostBlock = new System.Drawing.Size(vxsizeGhost, vysizeGhost);

                    // rysowanie blokow
                    int IndeksDucha = 0;
                    int IndeksKoloru = 0;
                    for (int iy = 0; iy < z.PrjGhostSize.Height; iy++)
                    {
                        for (int ix = 0; ix < z.PrjGhostSize.Width; ix++)
                        {

                            if (IndeksDucha < z.Duch0.Length)
                            {

                                //int znak = 0;
                                //int color = 0;

                                //z.Frame f = z.Frames[z.AnimationFrame];
                                //znak = f.Duch0[IndeksDucha];
                                //color = f.Duch0Color[iy];


                                int znakold = z.Frames[z.AnimationFrame].Duch3[IndeksDucha];
                                int colorold = z.Frames[z.AnimationFrame].Duch3Color[IndeksKoloru];
                                ShowFrame_GenerateBox(grp, iy, ix, znakold, colorold, colorold, false, znakold);
                                int znak = z.Frames[z.AnimationFrame].Duch2[IndeksDucha];
                                int color = z.Frames[z.AnimationFrame].Duch2Color[IndeksKoloru];
                                ShowFrame_GenerateBox(grp, iy, ix, znak, color, colorold, true, znakold);

                                znakold = z.Frames[z.AnimationFrame].Duch1[IndeksDucha];
                                colorold = z.Frames[z.AnimationFrame].Duch1Color[IndeksKoloru];
                                ShowFrame_GenerateBox(grp, iy, ix, znakold, colorold, colorold, false, znakold);
                                znak = z.Frames[z.AnimationFrame].Duch0[IndeksDucha];
                                color = z.Frames[z.AnimationFrame].Duch0Color[IndeksKoloru];
                                ShowFrame_GenerateBox(grp, iy, ix, znak, color, colorold, true, znakold);



                                //if (znak == 1)
                                //{
                                //    z.atariRGB argb = (z.atariRGB)z.palette.GetByIndex(color);
                                //    SolidBrush sbb = new System.Drawing.SolidBrush(argb.GetColor());
                                //    grp.FillRectangle(sbb, ix * z.tmpGhostBlock.Width, iy * z.tmpGhostBlock.Height, z.tmpGhostBlock.Width, z.tmpGhostBlock.Height);
                                //    sbb.Dispose();
                                //}
                                IndeksDucha++;
                            }
                        }
                        IndeksKoloru++;
                    }

                    myPen.Dispose();
                }
                pb.Image = resultImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd wykonania funkcji " + FunctionName() + "\r\n\r\n" + ex.ToString());
                //throw new Exception("Błąd wykonania funkcji " + FunctionName() + "\r\n\r\n", ex);
            }
        }
        bool TimerOn = false;
        private void button3_Click(object sender, EventArgs e)
        {
            if (TimerOn)
                timer.Stop();
            else
                timer.Start();
            TimerOn = !TimerOn;
            button_AnimationPlay.Text = TimerOn ? "Stop" : "Play";
        }
        private void numericUpDown_AnimationSpeed_ValueChanged(object sender, EventArgs e)
        {
            timer.Interval = (int)(numericUpDown_AnimationSpeed.Value * 100);
        }
        private void hScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            z.ZaczynajOdFrame = hScrollBar.Value;
            Console.WriteLine(z.ZaczynajOdFrame);
            SprMoveButtonOnOff();
            CopyFrameToGhost();
            ShowGhost();
        }
        private void SprMoveButtonOnOff()
        {
            if (z.ZaczynajOdFrame+z.WybranaFrame == z.Frames.Count - 1)
                button_RightFrame.Enabled = false;
            else
                button_RightFrame.Enabled = true;
            if (z.ZaczynajOdFrame + z.WybranaFrame == 0)
                button_LeftFrame.Enabled = false;
            else
                button_LeftFrame.Enabled = true;
            if (z.Frames.Count == 1)
                button_DelFrame.Enabled = false;
            else
                button_DelFrame.Enabled = true;

        }
        private void hScrollBar_ValueChanged(object sender, EventArgs e)
        {
            hScrollBar_Scroll(sender, null);
        }
        private void button_GhostClear_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Sure to clear the Player?", "Caution!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                ClearGhost();
                CopyGhostToFrame();
                ShowGhost();
            }
        }
        private void button_GhostUp_Click(object sender, EventArgs e)
        {
            if (checkBox_Ghost0.Checked && !checkBox_GhostSecure0.Checked) LeayerUp(z.Duch0, z.Duch0Color);
            if (checkBox_Ghost1.Checked && !checkBox_GhostSecure1.Checked) LeayerUp(z.Duch1, z.Duch1Color);
            if (checkBox_Ghost2.Checked && !checkBox_GhostSecure2.Checked) LeayerUp(z.Duch2, z.Duch2Color);
            if (checkBox_Ghost3.Checked && !checkBox_GhostSecure3.Checked) LeayerUp(z.Duch3, z.Duch3Color);
            CopyGhostToFrame();
            ShowGhost();
        }
        private void LeayerUp(byte[] duch, byte[] duchC)
        {
            byte[] bufor = new byte[8];
            byte[] buforC = new byte[1];
            buforC[0] = duchC[0];

            for (int i = 0; i < 8; i++)
            {
                bufor[i] = duch[i];   
            }
            for (int iy = 1; iy < z.PrjGhostSize.Height; iy++)
            {
                for (int ix = 0; ix < 8; ix++)
                {
                    duch[(iy-1)*8+ix] = duch[iy*8+ix];
                }
                duchC[(iy - 1)] = duchC[iy];

            }
            for (int i = 0; i < 8; i++)
            {
                duch[(z.PrjGhostSize.Height-1) * 8 + i] = bufor[i];
            }
            duchC[z.PrjGhostSize.Height - 1] = buforC[0];
            //duch[z.PrjGhostSize.Height] = d0;
        }
        private void button_GhostDown_Click(object sender, EventArgs e)
        {
            if (checkBox_Ghost0.Checked && !checkBox_GhostSecure0.Checked) LeayerDown(z.Duch0,z.Duch0Color);
            if (checkBox_Ghost1.Checked && !checkBox_GhostSecure1.Checked) LeayerDown(z.Duch1, z.Duch1Color);
            if (checkBox_Ghost2.Checked && !checkBox_GhostSecure2.Checked) LeayerDown(z.Duch2, z.Duch2Color);
            if (checkBox_Ghost3.Checked && !checkBox_GhostSecure3.Checked) LeayerDown(z.Duch3, z.Duch3Color);
            CopyGhostToFrame();
            ShowGhost();
        }
        private void LeayerDown(byte[] duch, byte[] duchC)
        {
            byte[] bufor = new byte[8];
            byte[] buforC = new byte[1];
            buforC[0] = duchC[z.PrjGhostSize.Height - 1];

            for (int i = 0; i < 8; i++)
            {
                bufor[i] = duch[(z.PrjGhostSize.Height - 1) * 8 + i];
            }

            for (int iy = z.PrjGhostSize.Height-1; iy >= 0; iy--)
            {
                for (int ix = 0; ix < 8; ix++)
                {
                    duch[(iy+1) * 8 + ix] = duch[ (iy) * 8 + ix];
                }
                duchC[(iy + 1)  ] = duchC[(iy)  ];
            }
            
            for (int i = 0; i < 8; i++)
            {
                duch[i] = bufor[i];
            }
            duchC[0] = buforC[0];
        }
        private void button_GhostLeft_Click(object sender, EventArgs e)
        {
            if (checkBox_Ghost0.Checked && !checkBox_GhostSecure0.Checked) LeayerLeft(z.Duch0);
            if (checkBox_Ghost1.Checked && !checkBox_GhostSecure1.Checked) LeayerLeft(z.Duch1);
            if (checkBox_Ghost2.Checked && !checkBox_GhostSecure2.Checked) LeayerLeft(z.Duch2);
            if (checkBox_Ghost3.Checked && !checkBox_GhostSecure3.Checked) LeayerLeft(z.Duch3);
            CopyGhostToFrame();
            ShowGhost();
        }
        private void LeayerLeft(byte[] duch)
        {
            for (int iy = 0; iy < z.PrjGhostSize.Height; iy++)
            {
                byte b0 = duch[iy * 8];
                for (int ix = 1; ix < 8; ix++)
                {
                    duch[iy * 8 + ix -1] = duch[iy * 8 + ix];
                }
                duch[iy * 8 + 7] = b0;
            }
        }
        private void button_GhostRight_Click(object sender, EventArgs e)
        {
            if (checkBox_Ghost0.Checked && !checkBox_GhostSecure0.Checked) LeayerRight(z.Duch0);
            if (checkBox_Ghost1.Checked && !checkBox_GhostSecure1.Checked) LeayerRight(z.Duch1);
            if (checkBox_Ghost2.Checked && !checkBox_GhostSecure2.Checked) LeayerRight(z.Duch2);
            if (checkBox_Ghost3.Checked && !checkBox_GhostSecure3.Checked) LeayerRight(z.Duch3);
            CopyGhostToFrame();
            ShowGhost();
        }
        private void LeayerRight(byte[] duch)
        {
            for (int iy = 0; iy < z.PrjGhostSize.Height; iy++)
            {
                byte b0 = duch[iy * 8+7];
                for (int ix = 6; ix >=0; ix--)
                {
                    duch[iy * 8 + ix + 1] = duch[iy * 8 + ix];
                }
                duch[iy * 8 ] = b0;
            }
        }
        private void pictureBox_palette_Click(object sender, EventArgs e)
        {

        }
        private void checkBox_ShowIndex_CheckedChanged(object sender, EventArgs e)
        {
            ShowPalette(comboBox_Palette.Text);
        }
        private void comboBox_Palette_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!z.FirstLoad)
            {
                ShowPalette(comboBox_Palette.Text);
                ShowGhost();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            About ab = new About();
            ab.ShowDialog();
        }
        private void ExportBins(string FileName)
        {
            string plik = FileName.Replace(".bin", "");
            ExportBin_Ghost(0, plik + "_Ghost0.bin");
            ExportBin_Ghost(1, plik + "_Ghost1.bin");
            ExportBin_Ghost(2, plik + "_Ghost2.bin");
            ExportBin_Ghost(3, plik + "_Ghost3.bin");
            ExportBin_Color(0, plik + "_Color0.bin");
            ExportBin_Color(1, plik + "_Color1.bin");
            ExportBin_Color(2, plik + "_Color2.bin");
            ExportBin_Color(3, plik + "_Color3.bin");
        }
        private void ExportBin_Ghost(int GhostIndeks, string FileName)
        {
            FileStream fw = File.OpenWrite(FileName);
            byte[] bity = new byte[z.PrjGhostSize.Height];
            int bit = 0;
            for (int i = 0; i < z.PrjGhostSize.Height; i++)
            {
                bit = 0;
                for (int ix = 0; ix < 8; ix++)
                {
                    bit = bit << 1;
                    if (GhostIndeks == 0) bit += z.Duch0[i * 8 + ix];
                    if (GhostIndeks == 1) bit += z.Duch1[i * 8 + ix];
                    if (GhostIndeks == 2) bit += z.Duch2[i * 8 + ix];
                    if (GhostIndeks == 3) bit += z.Duch3[i * 8 + ix];
                }
                bity[i] = (byte)bit;
            }
            fw.Write(bity, 0, bity.Length);
            fw.Close();
        }

        private void ExportBin_Color(int GhostIndeks, string FileName)
        {
            FileStream fw = File.OpenWrite(FileName);
            byte[] bity = new byte[z.PrjGhostSize.Height];
            for (int i = 0; i < z.PrjGhostSize.Height; i++)
            {
                if (GhostIndeks == 0) bity[i] = z.Duch0Color[i];
                if (GhostIndeks == 1) bity[i] = z.Duch1Color[i];
                if (GhostIndeks == 2) bity[i] = z.Duch2Color[i];
                if (GhostIndeks == 3) bity[i] = z.Duch3Color[i];
            }
            fw.Write(bity, 0, bity.Length);
            fw.Close();
        }


        private byte[] ExportGhostData(int AtFrame, int GhostIndeks)
        {
            byte[] bity = new byte[z.PrjGhostSize.Height];
            int bit = 0;
            for (int i = 0; i < z.PrjGhostSize.Height; i++)
            {
                bit = 0;
                for (int ix = 0; ix < 8; ix++)
                {
                    bit = bit << 1;
                    z.Frame f = new z.Frame();
                    f = z.Frames[AtFrame];
                    if (GhostIndeks == 0) bit += f.Duch0[i * 8 + ix];
                    if (GhostIndeks == 1) bit += f.Duch1[i * 8 + ix];
                    if (GhostIndeks == 2) bit += f.Duch2[i * 8 + ix];
                    if (GhostIndeks == 3) bit += f.Duch3[i * 8 + ix];
                }
                bity[i] = (byte)bit;
            }
            return bity;
        }
        private byte[] ExportGhostDataColor(int atFrame, int GhostIndeks, bool OnlyFirsLine)
        {
            int dlugosc = OnlyFirsLine ? 1 : z.PrjGhostSize.Height;
            byte[] bity = new byte[dlugosc];
            for (int i = 0; i < dlugosc; i++)
            {
                z.Frame f = new z.Frame();
                f = z.Frames[atFrame];
                if (GhostIndeks == 0) bity[i] = f.Duch0Color[i];
                if (GhostIndeks == 1) bity[i] = f.Duch1Color[i];
                if (GhostIndeks == 2) bity[i] = f.Duch2Color[i];
                if (GhostIndeks == 3) bity[i] = f.Duch3Color[i];
            }
            return bity;
        }


        // jedna klatka dla G2Font
        private void currentFrameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "G2F Sprites|*.pmg";
            saveFileDialog.Title = "Plik eksportu";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                byte[] duszek0 = ExportGhostData(0, 0);
                byte[] duszek1 = ExportGhostData(0, 1);
                byte[] duszek2 = ExportGhostData(0, 2);
                byte[] duszek3 = ExportGhostData(0, 3);
                // dopelnienie dla formatu G2Font
                byte[] duszek = new byte[1280];
                for (int i = 0; i < z.PrjGhostSize.Height; i++)
                {
                    duszek[255 + i] = duszek0[i];
                    duszek[512 + i] = duszek1[i];
                    duszek[768 + i] = duszek2[i];
                    duszek[1024 + i] = duszek3[i];
                }

                FileStream fw = File.OpenWrite(saveFileDialog.FileName);
                fw.Write(duszek, 0, duszek.Length);
                fw.Close();
            }
        }
        private void firstColorOfLineToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        #region Export
        // Bin - aktualna ramke (wszyscy gracze w jednym pliku)
        private void oneFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "Binary Dan Casper Editor|*.bin";
            saveFileDialog.Title = "Plik eksportu";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                byte[] duszek0 = new byte[z.PrjGhostSize.Height];
                if(checkBox_Ghost0.Checked) ExportGhostData(z.WybranaFrame,0);
                byte[] duszek1 = new byte[z.PrjGhostSize.Height]; 
                if(checkBox_Ghost1.Checked) ExportGhostData(z.WybranaFrame,1);
                byte[] duszek2 = new byte[z.PrjGhostSize.Height];
                if(checkBox_Ghost3.Checked) ExportGhostData(z.WybranaFrame,2);
                byte[] duszek3 = new byte[z.PrjGhostSize.Height];
                if (checkBox_Ghost3.Checked) ExportGhostData(z.WybranaFrame, 3);
                FileStream fw = File.OpenWrite(saveFileDialog.FileName);
                if (checkBox_Ghost0.Checked) fw.Write(duszek0, 0, duszek0.Length);
                if (checkBox_Ghost1.Checked) fw.Write(duszek1, 0, duszek1.Length);
                if (checkBox_Ghost2.Checked) fw.Write(duszek2, 0, duszek2.Length);
                if (checkBox_Ghost3.Checked) fw.Write(duszek3, 0, duszek3.Length);
                fw.Close();
            }
        }
        // Bin - aktualna ramke - gracze w osobnych plikach
        private void separatedFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "Binary Dan Casper Editor|*.bin";
            saveFileDialog.Title = "Plik eksportu";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                if (checkBox_Ghost0.Checked)
                {
                    FileStream fw = File.OpenWrite(FileNameIndeks(saveFileDialog.FileName, 0, z.WybranaFrame));
                    byte[] duszek = ExportGhostData(z.WybranaFrame,0);
                    fw.Write(duszek, 0, duszek.Length);
                    fw.Close();
                }

                if (checkBox_Ghost1.Checked)
                {
                    FileStream fw = File.OpenWrite(FileNameIndeks(saveFileDialog.FileName, 1, z.WybranaFrame));
                    byte[] duszek = ExportGhostData(z.WybranaFrame,1);
                    fw.Write(duszek, 0, duszek.Length);
                    fw.Close();
                }
                if (checkBox_Ghost2.Checked)
                {
                    FileStream fw = File.OpenWrite(FileNameIndeks(saveFileDialog.FileName, 2, z.WybranaFrame));
                    byte[] duszek = ExportGhostData( z.WybranaFrame,2);
                    fw.Write(duszek, 0, duszek.Length);
                    fw.Close();
                }
                if (checkBox_Ghost3.Checked)
                {
                    FileStream fw = File.OpenWrite(FileNameIndeks(saveFileDialog.FileName, 3, z.WybranaFrame));
                    byte[] duszek = ExportGhostData(z.WybranaFrame,3);
                    fw.Write(duszek, 0, duszek.Length);
                    fw.Close();
                }
            }
        }
        // Bin - wszystkie ramki -  jeden plik dla wszystkich duszków
        private void oneFileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "Binary Dan Casper Editor|*.bin";
            saveFileDialog.Title = "Plik eksportu";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                for (int i = 0; i < z.Frames.Count; i++)
                {
                    byte[] duszek0 = new byte[z.PrjGhostSize.Height];
                    if (checkBox_Ghost0.Checked) ExportGhostData(i, 0);
                    byte[] duszek1 = new byte[z.PrjGhostSize.Height];
                    if (checkBox_Ghost1.Checked) ExportGhostData(i, 1);
                    byte[] duszek2 = new byte[z.PrjGhostSize.Height];
                    if (checkBox_Ghost2.Checked) ExportGhostData(i, 2);
                    byte[] duszek3 = new byte[z.PrjGhostSize.Height];
                    if (checkBox_Ghost3.Checked) ExportGhostData(i, 3);

                    FileStream fw = File.OpenWrite(FileNameIndeks(saveFileDialog.FileName,i));
                    if (checkBox_Ghost0.Checked) fw.Write(duszek0, 0, duszek0.Length);
                    if (checkBox_Ghost1.Checked) fw.Write(duszek1, 0, duszek1.Length);
                    if (checkBox_Ghost2.Checked) fw.Write(duszek2, 0, duszek2.Length);
                    if (checkBox_Ghost3.Checked) fw.Write(duszek3, 0, duszek3.Length);
                    fw.Close();
                }
            }
        }
        private void separatedFileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "Binary Dan Casper Editor|*.bin";
            saveFileDialog.Title = "Plik eksportu";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                for (int i = 0; i < z.Frames.Count; i++)
                {

                    if (checkBox_Ghost0.Checked)
                    {
                        FileStream fw = File.OpenWrite(FileNameIndeks(saveFileDialog.FileName, 0, i));
                        byte[] duszek = ExportGhostData(i,0);
                        fw.Write(duszek, 0, duszek.Length);
                        fw.Close();
                    }

                    if (checkBox_Ghost1.Checked)
                    {
                        FileStream fw = File.OpenWrite(FileNameIndeks(saveFileDialog.FileName, 1, i));
                        byte[] duszek = ExportGhostData(i,1);
                        fw.Write(duszek, 0, duszek.Length);
                        fw.Close();
                    }
                    if (checkBox_Ghost2.Checked)
                    {
                        FileStream fw = File.OpenWrite(FileNameIndeks(saveFileDialog.FileName, 2, i));
                        byte[] duszek = ExportGhostData(i,2);
                        fw.Write(duszek, 0, duszek.Length);
                        fw.Close();
                    }
                    if (checkBox_Ghost3.Checked)
                    {
                        FileStream fw = File.OpenWrite(FileNameIndeks(saveFileDialog.FileName, 3, i));
                        byte[] duszek = ExportGhostData(i,3);
                        fw.Write(duszek, 0, duszek.Length);
                        fw.Close();
                    }
                }
            }
        }
        // Bin jeden blok
        private void oneBlockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "Binary Dan Casper Editor|*.bin";
            saveFileDialog.Title = "Plik eksportu";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileStream fw = File.OpenWrite(saveFileDialog.FileName);
                for (int i = 0; i < z.Frames.Count; i++)
                {
                    byte[] duszek0 = new byte[z.PrjGhostSize.Height];
                    if (checkBox_Ghost0.Checked) ExportGhostData(i, 0);
                    byte[] duszek1 = new byte[z.PrjGhostSize.Height];
                    if (checkBox_Ghost1.Checked) ExportGhostData(i, 1);
                    byte[] duszek2 = new byte[z.PrjGhostSize.Height];
                    if (checkBox_Ghost2.Checked) ExportGhostData(i, 2);
                    byte[] duszek3 = new byte[z.PrjGhostSize.Height];
                    if (checkBox_Ghost3.Checked) ExportGhostData(i, 3);

                    
                    if (checkBox_Ghost0.Checked) fw.Write(duszek0, 0, duszek0.Length);
                    if (checkBox_Ghost1.Checked) fw.Write(duszek1, 0, duszek1.Length);
                    if (checkBox_Ghost2.Checked) fw.Write(duszek2, 0, duszek2.Length);
                    if (checkBox_Ghost3.Checked) fw.Write(duszek3, 0, duszek3.Length);
                }
                fw.Close();
            }

        }
        #endregion


        private string FileNameIndeks(string Filename, int PlayersIndeks, int FrameIndeks)
        {
            string samanazwa = Path.GetFileNameWithoutExtension(Filename);
            Filename = Filename.Replace(Path.GetFileName(Filename), "");
            samanazwa += "_P" + PlayersIndeks.ToString("d1");
            samanazwa += "_F" + FrameIndeks.ToString("d2") + ".bin";
            samanazwa = Filename + samanazwa;
            return samanazwa;
        }
        private string FileNameIndeks(string Filename, int FrameIndeks)
        {
            string samanazwa = Path.GetFileNameWithoutExtension(Filename);
            Filename = Filename.Replace(Path.GetFileName(Filename), "");
            samanazwa += "_F" + FrameIndeks.ToString("d2") + ".bin";
            samanazwa = Filename + samanazwa;
            return samanazwa;
        }

        // color - wszystkie dane - aktualna ramka - jeden plik dla wszystkich graczy
        private void oneFileOnAllPlayersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "Binary Dan Casper Editor|*.bin";
            saveFileDialog.Title = "Plik eksportu";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                byte[] duszek0 = new byte[z.PrjGhostSize.Height];
                if (checkBox_Ghost0.Checked) ExportGhostDataColor(z.WybranaFrame, 0, false);
                byte[] duszek1 = new byte[z.PrjGhostSize.Height];
                if (checkBox_Ghost1.Checked) ExportGhostDataColor(z.WybranaFrame, 1, false);
                byte[] duszek2 = new byte[z.PrjGhostSize.Height];
                if (checkBox_Ghost3.Checked) ExportGhostDataColor(z.WybranaFrame, 2, false);
                byte[] duszek3 = new byte[z.PrjGhostSize.Height];
                if (checkBox_Ghost3.Checked) ExportGhostDataColor(z.WybranaFrame, 3, false);
                FileStream fw = File.OpenWrite(saveFileDialog.FileName);
                if (checkBox_Ghost0.Checked) fw.Write(duszek0, 0, duszek0.Length);
                if (checkBox_Ghost1.Checked) fw.Write(duszek1, 0, duszek1.Length);
                if (checkBox_Ghost2.Checked) fw.Write(duszek2, 0, duszek2.Length);
                if (checkBox_Ghost3.Checked) fw.Write(duszek3, 0, duszek3.Length);
                fw.Close();
            }
        }
        
        //private void separatedFileToolStripMenuItem_Click(object sender, EventArgs e)
        //{
           
        //}
        // color - wszystkie dane - wszystkie ramki - jeden plik
        private void oneFileOnAllPlayersToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "Binary Dan Casper Editor|*.bin";
            saveFileDialog.Title = "Plik eksportu";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                for (int i = 0; i < z.Frames.Count; i++)
                {
                    byte[] duszek0 = new byte[z.PrjGhostSize.Height];
                    if (checkBox_Ghost0.Checked) ExportGhostDataColor(i, 0, false);
                    byte[] duszek1 = new byte[z.PrjGhostSize.Height];
                    if (checkBox_Ghost1.Checked) ExportGhostDataColor(i, 1, false);
                    byte[] duszek2 = new byte[z.PrjGhostSize.Height];
                    if (checkBox_Ghost2.Checked) ExportGhostDataColor(i, 2, false);
                    byte[] duszek3 = new byte[z.PrjGhostSize.Height];
                    if (checkBox_Ghost3.Checked) ExportGhostDataColor(i, 3, false);

                    FileStream fw = File.OpenWrite(FileNameIndeks(saveFileDialog.FileName, i));
                    if (checkBox_Ghost0.Checked) fw.Write(duszek0, 0, duszek0.Length);
                    if (checkBox_Ghost1.Checked) fw.Write(duszek1, 0, duszek1.Length);
                    if (checkBox_Ghost2.Checked) fw.Write(duszek2, 0, duszek2.Length);
                    if (checkBox_Ghost3.Checked) fw.Write(duszek3, 0, duszek3.Length);
                    fw.Close();
                }
            }
        }
        // color - wszystkie dane - wszystkie ramki - osobne pliki
        private void separatedFileToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "Binary Dan Casper Editor|*.bin";
            saveFileDialog.Title = "Plik eksportu";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                for (int i = 0; i < z.Frames.Count; i++)
                {

                    if (checkBox_Ghost0.Checked)
                    {
                        FileStream fw = File.OpenWrite(FileNameIndeks(saveFileDialog.FileName, 0, i));
                        byte[] duszek = ExportGhostDataColor(i, 0, false);
                        fw.Write(duszek, 0, duszek.Length);
                        fw.Close();
                    }

                    if (checkBox_Ghost1.Checked)
                    {
                        FileStream fw = File.OpenWrite(FileNameIndeks(saveFileDialog.FileName, 1, i));
                        byte[] duszek = ExportGhostDataColor(i, 1, false);
                        fw.Write(duszek, 0, duszek.Length);
                        fw.Close();
                    }
                    if (checkBox_Ghost2.Checked)
                    {
                        FileStream fw = File.OpenWrite(FileNameIndeks(saveFileDialog.FileName, 2, i));
                        byte[] duszek = ExportGhostDataColor(i, 2, false);
                        fw.Write(duszek, 0, duszek.Length);
                        fw.Close();
                    }
                    if (checkBox_Ghost3.Checked)
                    {
                        FileStream fw = File.OpenWrite(FileNameIndeks(saveFileDialog.FileName, 3, i));
                        byte[] duszek = ExportGhostDataColor(i, 3, false);
                        fw.Write(duszek, 0, duszek.Length);
                        fw.Close();
                    }
                }
            }
        }
        // color - wszystkie dane - jeden blok danych
        private void oneBlockToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "Binary Dan Casper Editor|*.bin";
            saveFileDialog.Title = "Plik eksportu";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileStream fw = File.OpenWrite(saveFileDialog.FileName);
                for (int i = 0; i < z.Frames.Count; i++)
                {
                    byte[] duszek0 = new byte[z.PrjGhostSize.Height];
                    if (checkBox_Ghost0.Checked) ExportGhostDataColor(i, 0, false);
                    byte[] duszek1 = new byte[z.PrjGhostSize.Height];
                    if (checkBox_Ghost1.Checked) ExportGhostDataColor(i, 1, false);
                    byte[] duszek2 = new byte[z.PrjGhostSize.Height];
                    if (checkBox_Ghost2.Checked) ExportGhostDataColor(i, 2, false);
                    byte[] duszek3 = new byte[z.PrjGhostSize.Height];
                    if (checkBox_Ghost3.Checked) ExportGhostDataColor(i, 3, false);

                    if (checkBox_Ghost0.Checked) fw.Write(duszek0, 0, duszek0.Length);
                    if (checkBox_Ghost1.Checked) fw.Write(duszek1, 0, duszek1.Length);
                    if (checkBox_Ghost2.Checked) fw.Write(duszek2, 0, duszek2.Length);
                    if (checkBox_Ghost3.Checked) fw.Write(duszek3, 0, duszek3.Length);
                }
                fw.Close();
            }
        }
        
        // color - jeden kolor - aktualna ramka - jeden plik
        private void oneFileOnAllPlayersToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "Binary Dan Casper Editor|*.bin";
            saveFileDialog.Title = "Plik eksportu";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                byte[] duszek0 = new byte[1];
                if (checkBox_Ghost0.Checked) ExportGhostDataColor(z.WybranaFrame, 0, true);
                byte[] duszek1 = new byte[1];
                if (checkBox_Ghost1.Checked) ExportGhostDataColor(z.WybranaFrame, 1, true);
                byte[] duszek2 = new byte[1];
                if (checkBox_Ghost3.Checked) ExportGhostDataColor(z.WybranaFrame, 2, true);
                byte[] duszek3 = new byte[1];
                if (checkBox_Ghost3.Checked) ExportGhostDataColor(z.WybranaFrame, 3, true);
                FileStream fw = File.OpenWrite(saveFileDialog.FileName);
                if (checkBox_Ghost0.Checked) fw.Write(duszek0, 0, duszek0.Length);
                if (checkBox_Ghost1.Checked) fw.Write(duszek1, 0, duszek1.Length);
                if (checkBox_Ghost2.Checked) fw.Write(duszek2, 0, duszek2.Length);
                if (checkBox_Ghost3.Checked) fw.Write(duszek3, 0, duszek3.Length);
                fw.Close();
            }
        }
        // color - jeden kolor - aktualna ramka - osobne pliki
        private void separatedFileToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "Binary Dan Casper Editor|*.bin";
            saveFileDialog.Title = "Plik eksportu";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                if (checkBox_Ghost0.Checked)
                {
                    FileStream fw = File.OpenWrite(FileNameIndeks(saveFileDialog.FileName, 0, z.WybranaFrame));
                    byte[] duszek = ExportGhostDataColor(z.WybranaFrame, 0, true);
                    fw.Write(duszek, 0, duszek.Length);
                    fw.Close();
                }

                if (checkBox_Ghost1.Checked)
                {
                    FileStream fw = File.OpenWrite(FileNameIndeks(saveFileDialog.FileName, 1, z.WybranaFrame));
                    byte[] duszek = ExportGhostDataColor(z.WybranaFrame, 1, true);
                    fw.Write(duszek, 0, duszek.Length);
                    fw.Close();
                }
                if (checkBox_Ghost2.Checked)
                {
                    FileStream fw = File.OpenWrite(FileNameIndeks(saveFileDialog.FileName, 2, z.WybranaFrame));
                    byte[] duszek = ExportGhostDataColor(z.WybranaFrame, 2, true);
                    fw.Write(duszek, 0, duszek.Length);
                    fw.Close();
                }
                if (checkBox_Ghost3.Checked)
                {
                    FileStream fw = File.OpenWrite(FileNameIndeks(saveFileDialog.FileName, 3, z.WybranaFrame));
                    byte[] duszek = ExportGhostDataColor(z.WybranaFrame, 3, true);
                    fw.Write(duszek, 0, duszek.Length);
                    fw.Close();
                }
            }
        }
        // color - jeden kolor - wszystkie ramki - jeden plik
        private void oneFileOnAllPlayersToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "Binary Dan Casper Editor|*.bin";
            saveFileDialog.Title = "Plik eksportu";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                for (int i = 0; i < z.Frames.Count; i++)
                {
                    byte[] duszek0 = new byte[1];
                    if (checkBox_Ghost0.Checked) ExportGhostDataColor(i, 0, true);
                    byte[] duszek1 = new byte[1];
                    if (checkBox_Ghost1.Checked) ExportGhostDataColor(i, 1, true);
                    byte[] duszek2 = new byte[1];
                    if (checkBox_Ghost2.Checked) ExportGhostDataColor(i, 2, true);
                    byte[] duszek3 = new byte[1];
                    if (checkBox_Ghost3.Checked) ExportGhostDataColor(i, 3, true);

                    FileStream fw = File.OpenWrite(FileNameIndeks(saveFileDialog.FileName, i));
                    if (checkBox_Ghost0.Checked) fw.Write(duszek0, 0, duszek0.Length);
                    if (checkBox_Ghost1.Checked) fw.Write(duszek1, 0, duszek1.Length);
                    if (checkBox_Ghost2.Checked) fw.Write(duszek2, 0, duszek2.Length);
                    if (checkBox_Ghost3.Checked) fw.Write(duszek3, 0, duszek3.Length);
                    fw.Close();
                }
            }
        }
        // color - jeden kolor - wszystkie ramki - osobne pliki
        private void separatedFileToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "Binary Dan Casper Editor|*.bin";
            saveFileDialog.Title = "Plik eksportu";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                for (int i = 0; i < z.Frames.Count; i++)
                {

                    if (checkBox_Ghost0.Checked)
                    {
                        FileStream fw = File.OpenWrite(FileNameIndeks(saveFileDialog.FileName, 0, i));
                        byte[] duszek = ExportGhostDataColor(i, 0, true);
                        fw.Write(duszek, 0, duszek.Length);
                        fw.Close();
                    }

                    if (checkBox_Ghost1.Checked)
                    {
                        FileStream fw = File.OpenWrite(FileNameIndeks(saveFileDialog.FileName, 1, i));
                        byte[] duszek = ExportGhostDataColor(i, 1, true);
                        fw.Write(duszek, 0, duszek.Length);
                        fw.Close();
                    }
                    if (checkBox_Ghost2.Checked)
                    {
                        FileStream fw = File.OpenWrite(FileNameIndeks(saveFileDialog.FileName, 2, i));
                        byte[] duszek = ExportGhostDataColor(i, 2, true);
                        fw.Write(duszek, 0, duszek.Length);
                        fw.Close();
                    }
                    if (checkBox_Ghost3.Checked)
                    {
                        FileStream fw = File.OpenWrite(FileNameIndeks(saveFileDialog.FileName, 3, i));
                        byte[] duszek = ExportGhostDataColor(i, 3, true);
                        fw.Write(duszek, 0, duszek.Length);
                        fw.Close();
                    }
                }
            }
        }
        // color - jeden kolor - wszystkie ramki - one block
        private void oneBlockToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "Binary Dan Casper Editor|*.bin";
            saveFileDialog.Title = "Plik eksportu";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileStream fw = File.OpenWrite(saveFileDialog.FileName);
                for (int i = 0; i < z.Frames.Count; i++)
                {
                    byte[] duszek0 = new byte[1];
                    if (checkBox_Ghost0.Checked) ExportGhostDataColor(i, 0, true);
                    byte[] duszek1 = new byte[1];
                    if (checkBox_Ghost1.Checked) ExportGhostDataColor(i, 1, true);
                    byte[] duszek2 = new byte[1];
                    if (checkBox_Ghost2.Checked) ExportGhostDataColor(i, 2, true);
                    byte[] duszek3 = new byte[1];
                    if (checkBox_Ghost3.Checked) ExportGhostDataColor(i, 3, true);

                    if (checkBox_Ghost0.Checked) fw.Write(duszek0, 0, duszek0.Length);
                    if (checkBox_Ghost1.Checked) fw.Write(duszek1, 0, duszek1.Length);
                    if (checkBox_Ghost2.Checked) fw.Write(duszek2, 0, duszek2.Length);
                    if (checkBox_Ghost3.Checked) fw.Write(duszek3, 0, duszek3.Length);
                }
                fw.Close();
            }
        }
        // color - wszystkie dane - aktualna ramka - sobne pliki
        private void separatedFileToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "Binary Dan Casper Editor|*.bin";
            saveFileDialog.Title = "Plik eksportu";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                if (checkBox_Ghost0.Checked)
                {
                    FileStream fw = File.OpenWrite(FileNameIndeks(saveFileDialog.FileName, 0, z.WybranaFrame));
                    byte[] duszek = ExportGhostDataColor(z.WybranaFrame, 0, false);
                    fw.Write(duszek, 0, duszek.Length);
                    fw.Close();
                }

                if (checkBox_Ghost1.Checked)
                {
                    FileStream fw = File.OpenWrite(FileNameIndeks(saveFileDialog.FileName, 1, z.WybranaFrame));
                    byte[] duszek = ExportGhostDataColor(z.WybranaFrame, 1, false);
                    fw.Write(duszek, 0, duszek.Length);
                    fw.Close();
                }
                if (checkBox_Ghost2.Checked)
                {
                    FileStream fw = File.OpenWrite(FileNameIndeks(saveFileDialog.FileName, 2, z.WybranaFrame));
                    byte[] duszek = ExportGhostDataColor(z.WybranaFrame, 2, false);
                    fw.Write(duszek, 0, duszek.Length);
                    fw.Close();
                }
                if (checkBox_Ghost3.Checked)
                {
                    FileStream fw = File.OpenWrite(FileNameIndeks(saveFileDialog.FileName, 3, z.WybranaFrame));
                    byte[] duszek = ExportGhostDataColor(z.WybranaFrame, 3, false);
                    fw.Write(duszek, 0, duszek.Length);
                    fw.Close();
                }
            }
        }

    }
}
