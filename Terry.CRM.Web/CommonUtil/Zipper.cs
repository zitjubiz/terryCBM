using System;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;

namespace Terry.CRM.Web.CommonUtil
{
    public class Zipper
    {
        public void Zip(string FilePath)
        {

        }
        public void Zip(string FilePath, string ZipFileName)
        { 

        }
        public static void Zip(string FilePath, string ZipFileName,string Password)
        {
            //目标zip文件是否已经存在            
            //File.Delete(ZipFileName);

            // This setting will strip the leading part of the folder path in the entries, to
            // make the entries relative to the starting folder.
            // To include the full path for each entry up to the drive root, assign folderOffset = 0.
            int folderOffset = 0;
            string[] files;

            //路径是文件还是目录呢？
            if (File.Exists(FilePath))
            {
                files = new string[] { FilePath };
                folderOffset = FilePath.LastIndexOf("\\");
            }
            else
            {
                files = Directory.GetFiles(FilePath,"*.*",SearchOption.AllDirectories);
                folderOffset = FilePath.Length + (FilePath.EndsWith("\\") ? 0 : 1);
            }

            FileStream fsOut = File.Create(ZipFileName);
            ZipOutputStream zipStream = new ZipOutputStream(fsOut);

            zipStream.SetLevel(9); //0-9, 9 being the highest level of compression
            zipStream.Password = Password;	// optional. Null is the same as not setting. Required if using AES.
         

            foreach (string filename in files)
            {
                //如果目录的文件和目标zip文件重名，跳过
                if (new FileInfo(filename).FullName == new FileInfo(ZipFileName).FullName)
                    continue;

                FileInfo fi = new FileInfo(filename);

                string entryName = filename.Substring(folderOffset); // Makes the name in zip based on the folder
                entryName = ZipEntry.CleanName(entryName); // Removes drive from name and fixes slash direction
                ZipEntry newEntry = new ZipEntry(entryName);
                newEntry.DateTime = fi.LastWriteTime; // Note the zip format stores 2 second granularity

                // Specifying the AESKeySize triggers AES encryption. Allowable values are 0 (off), 128 or 256.
                // A password on the ZipOutputStream is required if using AES.
                newEntry.AESKeySize = 256;

                // To permit the zip to be unpacked by built-in extractor in WinXP and Server2003, WinZip 8, Java, and other older code,
                // you need to do one of the following: Specify UseZip64.Off, or set the Size.
                // If the file may be bigger than 4GB, or you do not need WinXP built-in compatibility, you do not need either,
                // but the zip will be in Zip64 format which not all utilities can understand.
                zipStream.UseZip64 = UseZip64.Off;
                newEntry.Size = fi.Length;

                zipStream.PutNextEntry(newEntry);

                // Zip the file in buffered chunks
                // the "using" will close the stream even if an exception occurs
                byte[] buffer = new byte[4096];
                using (FileStream streamReader = File.OpenRead(filename))
                {
                    StreamUtils.Copy(streamReader, zipStream, buffer);
                }
                zipStream.CloseEntry();
            }


            zipStream.IsStreamOwner = true;	// Makes the Close also Close the underlying stream
            zipStream.Close();
        }

        public void UnZip(string ZipFileName)
        {
            string destFile = "";
            try
            {
                ZipInputStream zipIn = new ZipInputStream(File.OpenRead(ZipFileName));
                string ExtractToPath = ".";
                ZipEntry entry;
                if ((entry = zipIn.GetNextEntry()) != null) //ONLY ONE FILE
                {
                    FileStream streamWriter = File.Create(ExtractToPath + entry.Name);
                    long size = entry.Size;
                    byte[] data = new byte[size];
                    while (true)
                    {
                        size = zipIn.Read(data, 0, data.Length);
                        if (size > 0) streamWriter.Write(data, 0, (int)size);
                        else break;
                    }
                    streamWriter.Close();
                    FileInfo fi = new FileInfo(ExtractToPath + entry.Name);
                    destFile = ExtractToPath + DateTime.Now.ToString("yyyyMMddHHmmss") + fi.Extension;
                    // RENAME FILE
                    if (File.Exists(destFile)) File.Delete(destFile);
                    File.Move(ExtractToPath + entry.Name, destFile);
                }
            }
            catch
            {
                Console.WriteLine("ERROR: Error while extract zip file.");
            }
            
        }

        public static void UnZip(string ZipFileName,string UnZipPath)
        {
            if (ZipFileName == string.Empty)
            {                
                return;
            }
            if (!File.Exists(ZipFileName))
            {
                return;
            }
            //解压文件夹为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹
            if (UnZipPath == string.Empty)
                UnZipPath = ZipFileName.Replace(Path.GetFileName(ZipFileName), Path.GetFileNameWithoutExtension(ZipFileName));
            if (!UnZipPath.EndsWith("\\"))
                UnZipPath += "\\";
            if (!Directory.Exists(UnZipPath))
                Directory.CreateDirectory(UnZipPath);

            try
            {
                using (ZipInputStream s = new ZipInputStream(File.OpenRead(ZipFileName)))
                {

                    ZipEntry theEntry;
                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        string directoryName = Path.GetDirectoryName(theEntry.Name);
                        string fileName = Path.GetFileName(theEntry.Name);
                        if (directoryName.Length > 0)
                        {
                            Directory.CreateDirectory(UnZipPath + directoryName);
                        }
                        if (!directoryName.EndsWith("\\"))
                            directoryName += "\\";
                        if (fileName != String.Empty)
                        {
                            using (FileStream streamWriter = File.Create(UnZipPath + theEntry.Name))
                            {

                                int size = 2048;
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    size = s.Read(data, 0, data.Length);
                                    if (size > 0)
                                    {
                                        streamWriter.Write(data, 0, size);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }//while
                }
            }
            catch (Exception ex)
            {
                
                return;
            }
            return;
        }//解压结束
        
        public void UnZip(string ZipFileName, string UnZipPath, string Password)
        {

        }
    }
}