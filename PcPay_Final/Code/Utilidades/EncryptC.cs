using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace PcPay.Code.Utilidades
{
    public static class EncryptC
    {
        internal static CryptoCOMLib.CryptoTripleDES CryptoTools = new CryptoCOMLib.CryptoTripleDES();
        internal static CryptoCOMLib.CryptoBase64 Base64 = new CryptoCOMLib.CryptoBase64();

        #region RC4

        public static string EncryptRC4(string data, string llave,bool hexadecimal = true)
        {
            string encriptar = EnDeCrypt(llave, data);
            if(hexadecimal){
                encriptar = StrToHexStr(encriptar);
            }
            return encriptar;
        }

        public static string DecryptRC4(string data, string llave,bool hexadecimal = true)
        {
            string text = string.Empty;

            if(hexadecimal)text = HexStrToStr(data);
            text = EnDeCrypt(llave, text);

            return text;
        }

        private static string EnDeCrypt(string password, string text)
        {
            const int N = 256;
            int[] sbox;

            sbox = new int[N];
            int[] key = new int[N];
            int n = password.Length;
            for (int a = 0; a < N; a++)
            {
                key[a] = (int)password[a % n];
                sbox[a] = a;
            }

            int b = 0;
            for (int a = 0; a < N; a++)
            {
                b = (b + sbox[a] + key[a]) % N;
                int tempSwap = sbox[a];
                sbox[a] = sbox[b];
                sbox[b] = tempSwap;
            }

            int i = 0, j = 0, k = 0;
            StringBuilder cipher = new StringBuilder();
            for (int a = 0; a < text.Length; a++)
            {
                i = (i + 1) % N;
                j = (j + sbox[i]) % N;
                int tempSwap = sbox[i];
                sbox[i] = sbox[j];
                sbox[j] = tempSwap;

                k = sbox[(sbox[i] + sbox[j]) % N];
                int cipherBy = ((int)text[a]) ^ k;
                cipher.Append(Convert.ToChar(cipherBy));
            }

            return cipher.ToString();

        }

        private static string StrToHexStr(string str)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                int v = Convert.ToInt32(str[i]);
                sb.Append(string.Format("{0:X2}", v));
            }
            return sb.ToString();
        }

        private static string HexStrToStr(string hexStr)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hexStr.Length; i += 2)
            {
                int n = Convert.ToInt32(hexStr.Substring(i, 2), 16);
                sb.Append(Convert.ToChar(n));
            }
            return sb.ToString();
        }


        #endregion

        #region AES

        /// <summary>
        /// Obtiene una llave AES de manera aleatoria
        /// </summary>
        /// <returns></returns>
        public static string ObtieneRandomAESKey()
        {
            RijndaelManaged myRijndael = new RijndaelManaged();

            myRijndael.Mode = CipherMode.CBC;
            myRijndael.KeySize = 128;
            myRijndael.Padding = PaddingMode.PKCS7;
            myRijndael.BlockSize = 128;
            myRijndael.FeedbackSize = 128;

            myRijndael.GenerateKey();
            myRijndael.GenerateIV();

            return ByteArrayToString(myRijndael.Key).ToUpper();
        }

        public static string encrypInAES128(string strKey, string strText)
        {
            try
            {

                string original = strText.Replace("\r\n", "");

                // Create a new instance of the Aes
                // class.  This generates a new key and initialization 
                // vector (IV).
                using (Aes myAes = Aes.Create())
                {
                    myAes.Mode = CipherMode.CBC;
                    myAes.KeySize = 128;
                    myAes.Padding = PaddingMode.PKCS7;
                    myAes.BlockSize = 128;
                    myAes.FeedbackSize = 128;
                    byte[] key = new byte[] { };
                    String result = "";

                    string str = strKey;

                    key = StringToByteArray(str);

                    myAes.Key = key;

                    // Encrypt the string to an array of bytes.
                    byte[] encrypted = EncryptStringToBytes_Aes128(original, myAes.Key, myAes.IV);

                    byte[] resultado = new byte[encrypted.Length + myAes.IV.Length];
                    Array.Copy(myAes.IV, 0, resultado, 0, myAes.IV.Length);
                    Array.Copy(encrypted, 0, resultado, myAes.IV.Length, encrypted.Length);
                    String textBase64 = System.Convert.ToBase64String(resultado);
                    return result = textBase64;

                    Int32 ii;

                    result = "";
                    ii = 0;
                    foreach (Byte tmp in encrypted)
                    {
                        result += tmp.ToString("X2");
                        ii++;
                        if ((ii % 4) == 0)
                            result += "";
                    }

                    return result;

                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return "";
            }
        }


        public static string descrypInAES128(string strKey, string strText)
        {
            try
            {

                string original = strText;

                // Create a new instance of the Aes
                // class.  This generates a new key and initialization 
                // vector (IV).
                using (Aes myAes = Aes.Create())
                {
                    myAes.Mode = CipherMode.CBC;
                    myAes.KeySize = 128;
                    myAes.Padding = PaddingMode.PKCS7;
                    myAes.BlockSize = 128;
                    myAes.FeedbackSize = 128;
                    byte[] key = new byte[] { };
                    byte[] xmlByte = new byte[] { };
                    String result = "";


                    string str = strKey;

                    key = StringToByteArray(str);

                    var base64EncodedBytes = System.Convert.FromBase64String(strText);
                    byte[] IVAES128 = new byte[16];
                    Array.Copy(base64EncodedBytes, 0, IVAES128, 0, 16);
                    myAes.IV = IVAES128;

                    base64EncodedBytes = System.Convert.FromBase64String(strText);
                    xmlByte = new byte[base64EncodedBytes.Length - 16];
                    Array.Copy(base64EncodedBytes, 16, xmlByte, 0, base64EncodedBytes.Length - 16);

                    myAes.Key = key;

                    // Encrypt the string to an array of bytes.
                    result = DecryptStringFromBytes_Aes128(xmlByte, myAes.Key, myAes.IV);

                    return result;

                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return "";
            }
        }



        #endregion

        #region RSA

        /// <summary>
        /// Función para encriptar una cadena con algoritmo RSA
        /// </summary>
        /// <param name="input"></param>
        /// <param name="llave"></param>
        /// <returns></returns>
        public static string EncryptRSA(string texto, string publicKey)
        {
            RSACryptoServiceProvider rsa = DecodeX509PublicKey(Convert.FromBase64String(publicKey));
            return (Convert.ToBase64String(rsa.Encrypt(Encoding.ASCII.GetBytes(texto), false)));
        }

        /// <summary>
        /// Desencripta una cadena con algoritmo RSA
        /// </summary>
        /// <param name="encryptedText"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public static string DecryptRSA(string encryptedText, string privateKey)
        {
            RSACryptoServiceProvider rsa = DecodeX509PublicKey(Convert.FromBase64String(privateKey));
            return (Convert.ToBase64String(rsa.Decrypt(Encoding.ASCII.GetBytes(encryptedText), false)));
        }

        #endregion


        #region  FUNCIONES EXTRAS

        private static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        //RSA
        private static RSACryptoServiceProvider DecodeX509PublicKey(byte[] x509key)
        {
            // encoded OID sequence for  PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"
            byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            byte[] seq = new byte[15];
            // ---------  Set up stream to read the asn.1 encoded SubjectPublicKeyInfo blob  ------
            MemoryStream mem = new MemoryStream(x509key);
            BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading
            byte bt = 0;
            ushort twobytes = 0;

            try
            {

                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();    //advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();   //advance 2 bytes
                else
                    return null;

                seq = binr.ReadBytes(15);       //read the Sequence OID
                if (!CompareBytearrays(seq, SeqOID))    //make sure Sequence for OID is correct
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8103) //data read as little endian order (actual data order for Bit String is 03 81)
                    binr.ReadByte();    //advance 1 byte
                else if (twobytes == 0x8203)
                    binr.ReadInt16();   //advance 2 bytes
                else
                    return null;

                bt = binr.ReadByte();
                if (bt != 0x00)     //expect null byte next
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();    //advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();   //advance 2 bytes
                else
                    return null;

                twobytes = binr.ReadUInt16();
                byte lowbyte = 0x00;
                byte highbyte = 0x00;

                if (twobytes == 0x8102) //data read as little endian order (actual data order for Integer is 02 81)
                    lowbyte = binr.ReadByte();  // read next bytes which is bytes in modulus
                else if (twobytes == 0x8202)
                {
                    highbyte = binr.ReadByte(); //advance 2 bytes
                    lowbyte = binr.ReadByte();
                }
                else
                    return null;
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };   //reverse byte order since asn.1 key uses big endian order
                int modsize = BitConverter.ToInt32(modint, 0);

                byte firstbyte = binr.ReadByte();
                binr.BaseStream.Seek(-1, SeekOrigin.Current);

                if (firstbyte == 0x00)
                {   //if first byte (highest order) of modulus is zero, don't include it
                    binr.ReadByte();    //skip this null byte
                    modsize -= 1;   //reduce modulus buffer size by 1
                }

                byte[] modulus = binr.ReadBytes(modsize);   //read the modulus bytes

                if (binr.ReadByte() != 0x02)            //expect an Integer for the exponent data
                    return null;
                int expbytes = (int)binr.ReadByte();        // should only need one byte for actual exponent data (for all useful values)
                byte[] exponent = binr.ReadBytes(expbytes);

                // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                RSAParameters RSAKeyInfo = new RSAParameters();
                RSAKeyInfo.Modulus = modulus;
                RSAKeyInfo.Exponent = exponent;
                RSA.ImportParameters(RSAKeyInfo);
                return RSA;
            }
            catch (Exception)
            {
                return null;
            }

            finally { binr.Close(); }

        }

        private static bool CompareBytearrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;
            int i = 0;
            foreach (byte c in a)
            {
                if (c != b[i])
                    return false;
                i++;
            }
            return true;
        }

        private static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }


        private static byte[] EncryptStringToBytes_Aes128(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");
            byte[] encrypted;
            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.KeySize = 128;
                aesAlg.Padding = PaddingMode.PKCS7;
                aesAlg.BlockSize = 128;
                aesAlg.FeedbackSize = 128;
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        private static string DecryptStringFromBytes_Aes128(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.KeySize = 128;
                aesAlg.Padding = PaddingMode.PKCS7;
                aesAlg.BlockSize = 128;
                aesAlg.FeedbackSize = 128;
                aesAlg.Key = Key;
                aesAlg.IV = IV;


                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }
            return plaintext;
        }

        #endregion

        #region Encriptacion TripleDES
        public static string EncryptTripleDES(string Text,string Key) {
            
            string valor = string.Empty;
            CryptoTools.DeriveKeyFromPassword(Key,null,1000);
            CryptoTools.EncryptText(Text);
            valor = Encriptar(Base64.Encrypt(CryptoTools.Result));
            return valor;
        
        }

        internal static string DecryptTripleDES(string text, string key)
        {
            text = Desencriptar(text);
            Base64.Decrypt(text);
            CryptoTools.DeriveKeyFromPassword(key, null, 1000);
            return CryptoTools.DecryptText(Base64.Result);
        }

        public static string Encriptar(string DataValue)
        {
            int x;
            string temp = string.Empty;
            int tempNum;
            string tempChar = string.Empty;
            char tempChar2;

            for (x = 0; x < DataValue.Length; x++)
            {
                tempChar2 = Convert.ToChar(DataValue.Substring(x, 1));
                tempNum = tempChar2 / 16;
                if ((tempNum * 16) < (int)tempChar2)
                {
                    int b = ((int)tempChar2 - (tempNum * 16));
                    tempChar = ConvToHex(b);
                    temp = temp + ConvToHex(tempNum) + tempChar;
                }
                else
                {
                    temp = temp + ConvToHex(tempNum) + "0";
                }

            }
            return temp;

        }
        private static string ConvToHex(int x)
        {
            if (x > 9)
            {
                return (char)(x + 55) + "";
            }
            else
            {
                return Convert.ToString(x);
            }

        }
        public static string Desencriptar(string DataValue)
        {
            int x;
            string Temp = string.Empty;
            string HexByte;
            for (x = 0; x < DataValue.Length; x += 2)
            {
                HexByte = DataValue.Substring(x,2);
                Temp = Temp + (char)ConvToInt(HexByte);
            }
            return Temp;
        }
        private static int ConvToInt(string x)
        {
            string x1, x2;
            int temp = 0;
            x1 = x.Substring(0, 1);
            x2 = x.Substring(1,1);

            int numero = 0;
            if (int.TryParse(x1,out numero))
            {
                temp = 16 * Convert.ToInt32(x1);
            }
            else
            {

                temp = (Convert.ToChar(x1) - 55) * 16;
            }

            int numero2 = 0;
            if (int.TryParse(x2,out numero2))
            {
                temp = temp + Convert.ToInt32(x2);
            }
            else
            {
                // Temp = Temp + (Asc(x2) - 55)
                temp = temp + Convert.ToChar(x2) - 55;
            }
            return temp;
        }
        #endregion


        internal static string generarSemilla(string strCadenaOrigen)
        {
         
            var aux = Encoding.ASCII.GetBytes(strCadenaOrigen);
            var aux3 = "";
            foreach(var item in aux){
                aux3 += item;
            }    
            return aux3;
        
        }
    }
}
