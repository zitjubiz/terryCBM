using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Management;

namespace Terry.CRM.Service
{
    class SerialNumService
    {
        string prikey, pubkey;

        public  SerialNumService()
        {
            pubkey = "<RSAKeyValue><Modulus>xe3teTUwLgmbiwFJwWEQnshhKxgcasglGsfNVFTk0hdqKc9i7wb+gG7HOdPZLh65QyBcFfzdlrawwVkiPEL5kNTX1q3JW5J49mTVZqWd3w49reaLd8StHRYJdyGAL4ZovBhSTThETi+zYvgQ5SvCGkM6/xXOz+lkMaEgeFcjQQs=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            prikey = "<RSAKeyValue><Modulus>xe3teTUwLgmbiwFJwWEQnshhKxgcasglGsfNVFTk0hdqKc9i7wb+gG7HOdPZLh65QyBcFfzdlrawwVkiPEL5kNTX1q3JW5J49mTVZqWd3w49reaLd8StHRYJdyGAL4ZovBhSTThETi+zYvgQ5SvCGkM6/xXOz+lkMaEgeFcjQQs=</Modulus><Exponent>AQAB</Exponent><P>5flMAd7IrUTx92yomBdJBPDzp1Kclpaw4uXB1Ht+YXqwLW/9icI6mcv7d2O0kuVLSWj8DPZJol9V8AtvHkC3oQ==</P><Q>3FRA9UWcFrVPvGR5bewcL7YqkCMZlybV/t6nCH+gyMfbEvgk+p04F+j8WiHDykWj+BahjScjwyF5SGADbrfJKw==</Q><DP>b4WOU1XbERNfF3JM67xW/5ttPNX185zN2Ko8bbMZXWImr1IgrD5RNqXRo1rphVbGRKoxmIOSv7flr8uLrisKIQ==</DP><DQ>otSZlSq2qomgvgg7PaOLSS+F0TQ/i1emO0/tffhkqT4ah7BgE97xP6puJWZivjAteAGxrxHH+kPY0EY1AzRMNQ==</DQ><InverseQ>Sxyz0fEf5m7GrzAngLDRP/i+QDikJFfM6qPyr3Ub6Y5RRsFbeOWY1tX3jmV31zv4cgJ6donH7W2dSBPi67sSsw==</InverseQ><D>nVqofsIgSZltxTcC8fA/DFz1kxMaFHKFvSK3RKIxQC1JQ3ASkUEYN/baAElB0f6u/oTNcNWVPOqE31IDe7ErQelVc4D26RgFd5V7dSsF3nVz00s4mq1qUBnCBLPIrdb0rcQZ8FUQTsd96qW8Foave4tm8vspbM65iVUBBVdSYYE=</D></RSAKeyValue>";

        }

        public string GetCpu()
        {
            string strCpu = null;
            ManagementClass myCpu = new ManagementClass("win32_Processor");
            ManagementObjectCollection myCpuConnection = myCpu.GetInstances();
            foreach (ManagementObject myObject in myCpuConnection)
            {
                strCpu = myObject.Properties["Processorid"].Value.ToString();
                break;
            }
            return strCpu;
        }

        public string GetDiskVolumeSerialNumber()
        {
            ManagementClass mc =
                 new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObject disk =
                 new ManagementObject("win32_logicaldisk.deviceid=\"d:\"");
            disk.Get();
            return disk.GetPropertyValue("VolumeSerialNumber").ToString();
        }
        public string Encypt(string OriginalText)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(prikey);
                //使用私钥来加密对象 
                RSAPKCS1SignatureFormatter f = new RSAPKCS1SignatureFormatter(rsa);
                f.SetHashAlgorithm("SHA1");

                byte[] source = System.Text.ASCIIEncoding.ASCII.GetBytes(OriginalText);
                SHA1Managed sha = new SHA1Managed();
                byte[] result = sha.ComputeHash(source);
                byte[] b = f.CreateSignature(result);
                
                return Convert.ToBase64String(b);
            }
        }
        public bool Validate(string EncyptedBase64Key)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(pubkey);
                RSAPKCS1SignatureDeformatter f = new RSAPKCS1SignatureDeformatter(rsa);
                f.SetHashAlgorithm("SHA1");

                byte[] key = Convert.FromBase64String(pubkey);
                SHA1Managed sha = new SHA1Managed();
                byte[] name = sha.ComputeHash(ASCIIEncoding.ASCII.GetBytes(EncyptedBase64Key));
                if (f.VerifySignature(name, key))
                    return true;
                else
                    return false;
            } 
        }

        void getKeys()
        {
            using (RSACryptoServiceProvider ras = new RSACryptoServiceProvider())
            {
                //公匙
                pubkey = ras.ToXmlString(false);
                System.Diagnostics.Debug.WriteLine(pubkey);
                //私匙
                prikey = ras.ToXmlString(true);
            }
        }
    }
}
