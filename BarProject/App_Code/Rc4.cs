using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BarProject.App_Code
{
    public class Rc4
    {
        #region Metodat Publike
        /// <summary>
        /// Kripton tekstin burim me celesin e kriptimit
        /// Vendos rezultatin ne tekstin e kriptuar 
        /// </summary>
        /// <returns>kthen true nqs me sukses</returns>
        public bool Kripto()
        {
            var kthe = true;

            try
            {

                // indekset qe perdoren me poshte
                long i = 0;
                long j = 0;

                // Vendos stringun input ne nje array byte-sh te perkohshem
                var encDefault = new UnicodeEncoding();
                var input = encDefault.GetBytes(_mSInClearText);

                // Array putput

                var output = new byte[input.Length];

                // Kopje lokale e m_nBoxLen
                //
                var nLocBox = new byte[MnBoxLen];
                MnBox.CopyTo(nLocBox, 0);

                //
                //	Len of Chipher
                //
                //
                // Ekzekutimi Algoritmit
                //
                for (long offset = 0; offset < input.Length; offset++)
                {
                    i = (i + 1) % MnBoxLen;
                    j = (j + nLocBox[i]) % MnBoxLen;
                    var temp = nLocBox[i];
                    nLocBox[i] = nLocBox[j];
                    nLocBox[j] = temp;
                    var a = input[offset];
                    var b = nLocBox[(nLocBox[i] + nLocBox[j]) % MnBoxLen];
                    output[offset] = (byte)(a ^ b);
                }

                // Vendos rezultatin ne stringun autput ( teksti kriptuar )
                //
                var outarrchar = new char[encDefault.GetCharCount(output, 0, output.Length)];
                encDefault.GetChars(output, 0, output.Length, outarrchar, 0);
                _mSCryptedText = new string(outarrchar);
            }
            catch
            {
                kthe = false;
            }

            return kthe;

        }

        /// <summary>
        /// Dekripton tekstin e kriptuar ne tekst burim me ane te celesit te kriptimit
        /// </summary>
        /// <returns>kthen true nqs me sukses</returns>
        public bool Dekripto()
        {
            // kthe perdoret per te ruajtur kthimin e funksionit
            var kthe = true;

            try
            {
                _mSInClearText = _mSCryptedText;
                _mSCryptedText = "";
                if (Kripto())
                {
                    _mSInClearText = _mSCryptedText;
                }
            }
            catch
            {
                // kthejme false me qe ndodhi gabim.
                kthe = false;
            }
            return kthe;
        }

        #endregion
        #region Përcaktimi propertive

        /// <summary>
        /// Kthen ose vendos celesin e kriptimit
        /// </summary>
        public string ÇelësiKriptimit
        {
            get { return _celes; }
            set
            {
                //
                // kryet vlerdhenie vetem nqs ka vlere te re
                //
                if (_celes != value)
                {
                    _celes = value;
                    // perdoret per te mbushur m_nBox
                    //
                    long index2 = 0;

                    // Krijon dy kodime te ndryshme
                    //
                    var ascii = Encoding.ASCII;
                    var unicode = Encoding.Unicode;

                    // Konverton celesin e kriptimit nga unicode ne ascii

                    var asciiBytes = Encoding.Convert(unicode, ascii, unicode.GetBytes(_celes));

                    // Konverton byte[] ne char[] dhe me pas ne string

                    var asciiChars = new char[ascii.GetCharCount(asciiBytes, 0, asciiBytes.Length)];
                    ascii.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0);
                    CelesAscii = new string(asciiChars);

                    // mbush m_nBox
                    long keyLen = _celes.Length;

                    // cikli i pare
                    for (long count = 0; count < MnBoxLen; count++)
                        MnBox[count] = (byte)count;

                    // cikli dyte
                    for (long count = 0; count < MnBoxLen; count++)
                    {
                        index2 = (index2 + MnBox[count] + asciiChars[count % keyLen]) % MnBoxLen;
                        var temp = MnBox[count];
                        MnBox[count] = MnBox[index2];
                        MnBox[index2] = temp;
                    }

                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string TekstiBurim
        {
            get { return _mSInClearText; }
            set
            {
                // kryen vlerdhenie nqs ka vlere te re
                if (_mSInClearText != null && _mSInClearText != value)
                    _mSInClearText = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string TekstiKriptuar
        {
            get { return _mSCryptedText; }
            set
            {
                // kryen vleredhenie nqs ka vlere te re
                if (_mSCryptedText != null)
                {
                    // ReSharper disable once RedundantCheckBeforeAssignment
                    if (_mSCryptedText != value)
                        _mSCryptedText = value;
                }
            }
        }

        #endregion

        #region Fushat private

        // celesi kriptimit ,versione Unicode & Ascii
        private string _celes = "";

        /// <summary>
        /// 
        /// </summary>
        public string CelesAscii = "";

        // eshte i lidhur me celesin e kriptimit
        protected byte[] MnBox = new byte[MnBoxLen];

        // gjatesia e nBox
        /// <summary>
        /// 
        /// </summary>
        public static long MnBoxLen = 255;

        // Teksti burim
        private string _mSInClearText = "";

        private string _mSCryptedText = "";

        #endregion
    }
}