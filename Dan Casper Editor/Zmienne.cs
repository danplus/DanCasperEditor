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


public static class z
{

    public static class Konfiguracja
    {
        public static string Serwer { get; set; }
        public static string Baza { get; set; }
        public static string Uzytkownik { get; set; }
        public static string Haslo { get; set; }
    }

    public static class Tymczasowe
    {
        public static SortedList slSortedList = new SortedList();
        public static ArrayList alArrayList = new ArrayList();
        public static bool bBool = new bool();
        public static int iInt = new int();
    }

    public static bool FirstLoad = true;

    public static int WidocznyLayers = 255;

    public static bool TrybWyboruTla = false;
    public static byte KolorTla = 0;
    public static Size PrjGhostSize;

    public static int AnimationFrame = 0;
    
    public static int WybranyLayers = 0;
    public static byte WybranyWierszKoloru = 0;
    public static int WybranaFrame = 0;
    public static int ZaczynajOdFrame = 0;
    
    public static Size GhostSize; 
    public static Size GhostBlock;
    public static Size GhostMargin;

    public static Size PaletteSize;
    public static Size PaletteBlock;
    public static Size PaletteMargin;

    public static Size tmpGhostSize;
    public static Size tmpGhostBlock;

    public static byte[] Duch0 = new byte[2048];
    public static byte[] Duch1 = new byte[2048];
    public static byte[] Duch2 = new byte[2048];
    public static byte[] Duch3 = new byte[2048];
    public static byte[] Duch0Color = new byte[256];
    public static byte[] Duch1Color = new byte[256];
    public static byte[] Duch2Color = new byte[256];
    public static byte[] Duch3Color = new byte[256];


    public static List<Frame> Frames = new List<Frame>();

    public class Frame
    {
        public byte[] Duch0 = new byte[2048];
        public byte[] Duch1 = new byte[2048];
        public byte[] Duch2 = new byte[2048];
        public byte[] Duch3 = new byte[2048];
        public byte[] Duch0Color = new byte[256];
        public byte[] Duch1Color = new byte[256];
        public byte[] Duch2Color = new byte[256];
        public byte[] Duch3Color = new byte[256];
    }


    public static SortedList palette = new SortedList();

    public class DaneUzytkownika_IndeksNazwa
    {
        private int _value;
        private string _name;
        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public DaneUzytkownika_IndeksNazwa(string name, int value)
        {
            _name = name;
            _value = value;
        }
        public override string ToString()
        {
            return _name;
        }
    }


    public class atariRGB
    {
        public atariRGB(byte bR, byte bG, byte bB)
        {
            R = bR;
            G = bG;
            B = bB;
        }
        byte R { get; set; }
        byte G { get; set; }
        byte B { get; set; }
        
        public System.Drawing.Color GetColor()
        {
            return System.Drawing.Color.FromArgb(R,G,B);
        }
    }
}