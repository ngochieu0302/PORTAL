using ESCS_PORTAL.COMMON.Contants;
using ESCS_PORTAL.COMMON.ExtensionMethods;
using ESCS_PORTAL.COMMON.Oracle;
using ESCS_PORTAL.COMMON.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ESCS_PORTAL.COMMON.Common
{
    public class Utilities
    {
        const string GS_KEY = "escs@2020";
        
        public static string Encrypt(string toEncrypt, bool useHashing = true)
        {
            if (string.IsNullOrEmpty(toEncrypt))
                return "";
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);


            //System.Windows.Forms.MessageBox.Show(key);
            //If hashing use get hashcode regards to your key
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(GS_KEY));
                //Always release the resources and flush data
                // of the Cryptographic service provide. Best Practice

                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(GS_KEY);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public static string Decrypt(string cipherString, bool useHashing = true)
        {
            if (string.IsNullOrEmpty(cipherString))
                return "";
            byte[] keyArray;
            //get the byte code of the string

            byte[] toEncryptArray = Convert.FromBase64String(cipherString);


            //Get your key from config file to open the lock!
            string key = GS_KEY;

            if (useHashing)
            {
                //if hashing was used get the hash code with regards to your key
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //release any resource held by the MD5CryptoServiceProvider

                hashmd5.Clear();
            }
            else
            {
                //if hashing was not implemented get the byte code of the key
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor                
            tdes.Clear();
            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        public static string EncryptByKey(string toEncrypt, string hashkey, bool useHashing = true)
        {
            if (string.IsNullOrEmpty(toEncrypt))
                return "";
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hashkey));
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(hashkey);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public static string DecryptByKey(string cipherString, string hashkey, bool useHashing = true)
        {
            try
            {
                if (string.IsNullOrEmpty(cipherString))
                    return "";
                byte[] keyArray;
                byte[] toEncryptArray = Convert.FromBase64String(cipherString);
                string key = hashkey;
                if (useHashing)
                {
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    hashmd5.Clear();
                }
                else
                {
                    keyArray = UTF8Encoding.UTF8.GetBytes(key);
                }
                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(
                                     toEncryptArray, 0, toEncryptArray.Length);
                tdes.Clear();
                return UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch
            {
                return null;
            }
        }

        public static string Sha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        public static string HMACSHA256(string message, string secret_key)
        {
            //secret = secret ?? "";
            //var encoding = new System.Text.ASCIIEncoding();
            //byte[] keyByte = encoding.GetBytes(secret_key);
            //byte[] messageBytes = encoding.GetBytes(message);
            //using (var hmacsha256 = new HMACSHA256(keyByte))
            //{
            //    byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
            //    return Convert.ToBase64String(hashmessage);
            //}
            byte[] bytesToSign = getBytes(message);
            byte[] secret = getBytes(secret_key);
            var alg = new HMACSHA256(secret);
            var hash = alg.ComputeHash(bytesToSign);
            var computedSignature = Base64UrlEncode(hash);
            return  computedSignature;
        }
        public static string HMACSHA1(string message, string secret)
        {
            secret = secret ?? "";
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha1 = new HMACSHA1(keyByte))
            {
                byte[] hashmessage = hmacsha1.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }
        private static string Base64UrlEncode(byte[] input)
        {
            var output = Convert.ToBase64String(input);
            output = output.Split('=')[0]; 
            output = output.Replace('+', '-'); 
            output = output.Replace('/', '_');
            return output;
        }
        private static byte[] getBytes(string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }
        public static Dictionary<string, string> ConvertStringJsonToDictionary(string json)
        {
            var dicObj = DeserializeData(json);
            if (dicObj != null && dicObj.Count() > 0)
            {
                Dictionary<string, string> dString = dicObj.ToDictionary(k => k.Key, k => k.Value == null ? "" : k.Value.ToString());
                return dString;
            }
            return null;
        }
        public static IDictionary<string, object> DeserializeData(string data)
        {
            var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
            return DeserializeData(values);
        }
        public static IDictionary<string, object> DeserializeData(JObject data)
        {
            var dict = data.ToObject<Dictionary<String, Object>>();
            return DeserializeData(dict);
        }
        public static IDictionary<string, object> DeserializeData(IDictionary<string, object> data)
        {
            foreach (var key in data.Keys.ToArray())
            {
                var value = data[key];

                if (value is JObject)
                    data[key] = DeserializeData(value as JObject);

                if (value is JArray)
                    data[key] = DeserializeData(value as JArray);
            }

            return data;
        }
        public static IList<Object> DeserializeData(JArray data)
        {
            var list = data.ToObject<List<Object>>();

            for (int i = 0; i < list.Count; i++)
            {
                var value = list[i];

                if (value is JObject)
                    list[i] = DeserializeData(value as JObject);

                if (value is JArray)
                    list[i] = DeserializeData(value as JArray);
            }

            return list;
        }
        public static int TinhTuoi(string ngay_sinh, string ngay_hl = null)
        {
            if (string.IsNullOrEmpty(ngay_hl))
            {
                ngay_hl = DateTime.Now.ToString("dd/MM/yyyy");
            }
            var arr_ngay_sinh = ngay_sinh.Split('/');
            var arr_ngay_hl = ngay_hl.Split('/');

            var ngay_sinh_year = int.Parse(arr_ngay_sinh[2]);
            var ngay_sinh_month = int.Parse(arr_ngay_sinh[1]);
            var ngay_sinh_day = int.Parse(arr_ngay_sinh[0]);

            var ngay_hl_year = int.Parse(arr_ngay_hl[2]);
            var ngay_hl_month = int.Parse(arr_ngay_hl[1]);
            var ngay_hl_day = int.Parse(arr_ngay_hl[0]);

            if (ngay_sinh_month == 0)
            {
                ngay_sinh_month++;
                ngay_hl_month++;
            }
            var numberOfMonths = (ngay_hl_year - ngay_sinh_year) * 12 + (ngay_hl_month - ngay_sinh_month);
            if (ngay_hl_month == ngay_sinh_month)
            {
                if (ngay_hl_day < ngay_sinh_day)
                {
                    numberOfMonths = numberOfMonths - 1;
                }
            }
            var age = Math.Floor((decimal)numberOfMonths / 12);
            return (int)age;
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        public static string Base64UrlEncode(string input)
        {
            var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            var output = Convert.ToBase64String(inputBytes);
            output = output.Split('=')[0]; 
            output = output.Replace('+', '-');
            output = output.Replace('/', '_');
            return output;
        }
        public static void SaveImage(Stream stream, string path, long QualityImage = 100)
        {
            Bitmap image = new Bitmap(stream);
            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, QualityImage);
            // JPEG image codec 
            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");
            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;
            image.Save(path, jpegCodec, encoderParams);
        }
        public static void SaveImageChangeSize(Stream stream, string path,int max_width, long QualityImage = 100)
        {
            byte[] imageBytes = StreamToByteArray(stream);
            using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                using (Image img = Image.FromStream(ms))
                {
                    var current_size = img.Size;
                    int height = current_size.Height;
                    if (current_size.Width> max_width)
                    {
                        height = max_width * height / current_size.Width;
                    }
                    using (Bitmap b = new Bitmap(img, new Size(max_width, height)))
                    {
                        EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, QualityImage);
                        // JPEG image codec 
                        ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");
                        EncoderParameters encoderParams = new EncoderParameters(1);
                        encoderParams.Param[0] = qualityParam;
                        b.Save(path, jpegCodec, encoderParams);
                    }
                }
            }
        }
        public static byte[] StreamToByteArray(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats 
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            // Find the correct image codec 
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];

            return null;
        }
        public static T ConvertStringJsonToObject<T>(string stringJson)
        {
            if (string.IsNullOrEmpty(stringJson))
            {
                return default(T);
            }
            JsonConvert.DefaultSettings = SettingMaxLengtJson;
            return JsonConvert.DeserializeObject<T>(stringJson);
        }
        public static string ConvertObjectToStringJson<T>(T obj)
        {
            JsonConvert.DefaultSettings = SettingMaxLengtJson;
            return JsonConvert.SerializeObject(obj);
        }
        public static JsonSerializerSettings SettingMaxLengtJson()
        {
            JsonSerializerSettings jsonSetting = new JsonSerializerSettings();
            jsonSetting.MaxDepth = int.MaxValue;
            return jsonSetting;
        }
        public static object ToObjectJson(dynamic data)
        {
            if (data == null)
            {
                return null;
            }
            String objJson = JsonConvert.SerializeObject(data, new JsonSerializerSettings() { Formatting = Formatting.Indented, ContractResolver = new LowercaseContractResolver(), MaxDepth = int.MaxValue });
            object json = JsonConvert.DeserializeObject(objJson, typeof(object));
            return json;
        }
        public static object ToListObjectJson(IEnumerable<dynamic> data)
        {
            if (data == null)
            {
                return null;
            }
            String objJson = JsonConvert.SerializeObject(data, new JsonSerializerSettings() { Formatting = Formatting.Indented, ContractResolver = new LowercaseContractResolver(), MaxDepth = int.MaxValue });
            object dataJson = JsonConvert.DeserializeObject(objJson, typeof(object));
            return dataJson;
        }
        public static bool IsCollectionType(Type type)
        {
            return (type.GetInterface(nameof(ICollection)) != null);
        }
        public static BaseRequest GetRequestData(object model, out Dictionary<string, bool> prefix_arr)
        {
            var json = JsonConvert.SerializeObject(model);
            IDictionary<string, object> data = Utilities.DeserializeData(json);
            object obj_data_info = data["data_info"];
            data["data_info"] = null;
            BaseRequest rq = data.GetObject<BaseRequest>();
            rq.data_info = new Dictionary<string, string>();
            prefix_arr = new Dictionary<string, bool>();
            if (obj_data_info == null)
            {
                return rq;
            }
            var json_data_info = JsonConvert.SerializeObject(obj_data_info);
            if (string.IsNullOrEmpty(json_data_info))
            {
                return rq;
            }
            IDictionary<string, object> data_info = Utilities.DeserializeData(json_data_info);
            if (data_info == null || data_info.Count() <= 0)
            {
                return rq;
            }
            foreach (var key in data_info.Keys.ToArray())
            {
                var value = data_info[key];
                bool isCollection = value == null ? false : Utilities.IsCollectionType(value.GetType());
                if (isCollection)
                {
                    bool isObject = false;
                    IEnumerable<object> lstData = value as IEnumerable<object>;
                    int index = 0;
                    foreach (var item in lstData)
                    {
                        if (Utilities.IsCollectionType(item.GetType()))
                        {
                            isObject = true;
                            var jsonItem = JsonConvert.SerializeObject(item);
                            IDictionary<string, object> dataItem = Utilities.DeserializeData(jsonItem);
                            if (dataItem != null && dataItem.Count() > 0)
                            {
                                foreach (var itemKey in dataItem.Keys)
                                {
                                    object valueItem = dataItem[itemKey];
                                    rq.data_info.Add(key + "[" + index + "][" + itemKey + "]", valueItem != null ? valueItem.ToString() : "");
                                    index++;
                                }
                            }
                        }
                        else
                        {
                            isObject = false;
                            rq.data_info.Add(key + "[" + index + "]", item != null ? item.ToString() : "");
                            index++;
                        }
                    }
                    prefix_arr.Add(key, isObject);
                }
                else
                {
                    rq.data_info.Add(key, value != null ? value.ToString() : "");
                }
            }
            return rq;
        }
        public static string GetKeyCache(ActionConnection action, BaseRequest rq = null)
        {
            if (action == null || string.IsNullOrEmpty(action.type_cache) || action.type_cache == "NONE")
                return null;

            switch (action.type_cache)
            {
                case "ALLOW_CACHE":
                    string KEY_ALLOW_CACHE = CachePrefixKeyConstants.GetKeyCacheResponseAction(action.env, action.partner_code, action.db_name, action.schemadb, action.actionprocode, action.action_key_cache);
                    string[] param_caches = string.IsNullOrEmpty(action.param_cache) ? null : action.param_cache.Split(',');
                    if (param_caches != null && param_caches.Count() > 0 && rq != null && rq.data_info != null && rq.data_info.Count() > 0)
                    {
                        string key_search = "";
                        foreach (var item in param_caches)
                        {
                            string key = item;
                            if (item.StartsWith("b_"))
                            {
                                key = item.Substring(2, item.Length - 2);
                            }
                            if (rq.data_info.ContainsKey(item))
                            {
                                var val = rq.data_info[item];
                                if (!string.IsNullOrEmpty(val))
                                {
                                    if (!string.IsNullOrEmpty(key_search))
                                        key_search = string.Format("{0}.{1}", key_search, key.ToUpper() + "[" + val + "]");
                                    else
                                        key_search = string.Format("{0}{1}", key_search, key.ToUpper() + "[" + val + "]");
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(key_search))
                        {
                            KEY_ALLOW_CACHE = CachePrefixKeyConstants.GetKeyCacheResponseAction(action.env, action.partner_code, action.db_name, action.schemadb, action.actionprocode, action.action_key_cache, key_search);
                        }
                    }
                    return KEY_ALLOW_CACHE;
                case "BEHAVIORS_CACHE":
                    return "";
                default:
                    return null;
            }
        }
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public static byte[] FileToByteArray(string fileName)
        {
            if (!System.IO.File.Exists(fileName))
            {
                return null;
            }
            byte[] fileContent = null;
            System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.BinaryReader binaryReader = new System.IO.BinaryReader(fs);
            long byteLength = new System.IO.FileInfo(fileName).Length;
            fileContent = binaryReader.ReadBytes((Int32)byteLength);
            fs.Close();
            fs.Dispose();
            binaryReader.Close();
            return fileContent;
        }
        public static void SaveFile(Stream stream, string destPath)
        {
            using (var fileStream = new FileStream(destPath, FileMode.Create, FileAccess.Write))
            {
                stream.CopyTo(fileStream);
            }
        }
        public static Image ResizeImage(System.Drawing.Image imgToResize, Size size)
        {
            //Get the image current width
            int sourceWidth = imgToResize.Width;
            //Get the image current height
            int sourceHeight = imgToResize.Height;
            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            //Calulate  width with new desired size
            nPercentW = ((float)size.Width / (float)sourceWidth);
            //Calculate height with new desired size
            nPercentH = ((float)size.Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;
            //New Width
            int destWidth = (int)(sourceWidth * nPercent);
            //New Height
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            // Draw image with new width and height
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return (System.Drawing.Image)b;
        }
        public static byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }
        public static Image byteArrayToImage(byte[] byteArrayIn)
        {
            try
            {
                using (var ms = new MemoryStream(byteArrayIn))
                {
                    return Image.FromStream(ms, true, true);
                }

            }
            catch { }
            return null;
        }

        public async static Task SaveFileAsync(Stream stream, string destPath)
        {
            using (var fileStream = new FileStream(destPath, FileMode.Create, FileAccess.Write))
            {
                await stream.CopyToAsync(fileStream);
            }
        }
        private static IDictionary<string, string> _mappings = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
        {
            {".323", "text/h323"},
            {".3g2", "video/3gpp2"},
            {".3gp", "video/3gpp"},
            {".3gp2", "video/3gpp2"},
            {".3gpp", "video/3gpp"},
            {".7z", "application/x-7z-compressed"},
            {".aa", "audio/audible"},
            {".AAC", "audio/aac"},
            {".aaf", "application/octet-stream"},
            {".aax", "audio/vnd.audible.aax"},
            {".ac3", "audio/ac3"},
            {".aca", "application/octet-stream"},
            {".accda", "application/msaccess.addin"},
            {".accdb", "application/msaccess"},
            {".accdc", "application/msaccess.cab"},
            {".accde", "application/msaccess"},
            {".accdr", "application/msaccess.runtime"},
            {".accdt", "application/msaccess"},
            {".accdw", "application/msaccess.webapplication"},
            {".accft", "application/msaccess.ftemplate"},
            {".acx", "application/internet-property-stream"},
            {".AddIn", "text/xml"},
            {".ade", "application/msaccess"},
            {".adobebridge", "application/x-bridge-url"},
            {".adp", "application/msaccess"},
            {".ADT", "audio/vnd.dlna.adts"},
            {".ADTS", "audio/aac"},
            {".afm", "application/octet-stream"},
            {".ai", "application/postscript"},
            {".aif", "audio/x-aiff"},
            {".aifc", "audio/aiff"},
            {".aiff", "audio/aiff"},
            {".air", "application/vnd.adobe.air-application-installer-package+zip"},
            {".amc", "application/x-mpeg"},
            {".application", "application/x-ms-application"},
            {".art", "image/x-jg"},
            {".asa", "application/xml"},
            {".asax", "application/xml"},
            {".ascx", "application/xml"},
            {".asd", "application/octet-stream"},
            {".asf", "video/x-ms-asf"},
            {".ashx", "application/xml"},
            {".asi", "application/octet-stream"},
            {".asm", "text/plain"},
            {".asmx", "application/xml"},
            {".aspx", "application/xml"},
            {".asr", "video/x-ms-asf"},
            {".asx", "video/x-ms-asf"},
            {".atom", "application/atom+xml"},
            {".au", "audio/basic"},
            {".avi", "video/x-msvideo"},
            {".axs", "application/olescript"},
            {".bas", "text/plain"},
            {".bcpio", "application/x-bcpio"},
            {".bin", "application/octet-stream"},
            {".bmp", "image/bmp"},
            {".c", "text/plain"},
            {".cab", "application/octet-stream"},
            {".caf", "audio/x-caf"},
            {".calx", "application/vnd.ms-office.calx"},
            {".cat", "application/vnd.ms-pki.seccat"},
            {".cc", "text/plain"},
            {".cd", "text/plain"},
            {".cdda", "audio/aiff"},
            {".cdf", "application/x-cdf"},
            {".cer", "application/x-x509-ca-cert"},
            {".chm", "application/octet-stream"},
            {".class", "application/x-java-applet"},
            {".clp", "application/x-msclip"},
            {".cmx", "image/x-cmx"},
            {".cnf", "text/plain"},
            {".cod", "image/cis-cod"},
            {".config", "application/xml"},
            {".contact", "text/x-ms-contact"},
            {".coverage", "application/xml"},
            {".cpio", "application/x-cpio"},
            {".cpp", "text/plain"},
            {".crd", "application/x-mscardfile"},
            {".crl", "application/pkix-crl"},
            {".crt", "application/x-x509-ca-cert"},
            {".cs", "text/plain"},
            {".csdproj", "text/plain"},
            {".csh", "application/x-csh"},
            {".csproj", "text/plain"},
            {".css", "text/css"},
            {".csv", "text/csv"},
            {".cur", "application/octet-stream"},
            {".cxx", "text/plain"},
            {".dat", "application/octet-stream"},
            {".datasource", "application/xml"},
            {".dbproj", "text/plain"},
            {".dcr", "application/x-director"},
            {".def", "text/plain"},
            {".deploy", "application/octet-stream"},
            {".der", "application/x-x509-ca-cert"},
            {".dgml", "application/xml"},
            {".dib", "image/bmp"},
            {".dif", "video/x-dv"},
            {".dir", "application/x-director"},
            {".disco", "text/xml"},
            {".dll", "application/x-msdownload"},
            {".dll.config", "text/xml"},
            {".dlm", "text/dlm"},
            {".doc", "application/msword"},
            {".docm", "application/vnd.ms-word.document.macroEnabled.12"},
            {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
            {".dot", "application/msword"},
            {".dotm", "application/vnd.ms-word.template.macroEnabled.12"},
            {".dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template"},
            {".dsp", "application/octet-stream"},
            {".dsw", "text/plain"},
            {".dtd", "text/xml"},
            {".dtsConfig", "text/xml"},
            {".dv", "video/x-dv"},
            {".dvi", "application/x-dvi"},
            {".dwf", "drawing/x-dwf"},
            {".dwp", "application/octet-stream"},
            {".dxr", "application/x-director"},
            {".eml", "message/rfc822"},
            {".emz", "application/octet-stream"},
            {".eot", "application/octet-stream"},
            {".eps", "application/postscript"},
            {".etl", "application/etl"},
            {".etx", "text/x-setext"},
            {".evy", "application/envoy"},
            {".exe", "application/octet-stream"},
            {".exe.config", "text/xml"},
            {".fdf", "application/vnd.fdf"},
            {".fif", "application/fractals"},
            {".filters", "Application/xml"},
            {".fla", "application/octet-stream"},
            {".flr", "x-world/x-vrml"},
            {".flv", "video/x-flv"},
            {".fsscript", "application/fsharp-script"},
            {".fsx", "application/fsharp-script"},
            {".generictest", "application/xml"},
            {".gif", "image/gif"},
            {".group", "text/x-ms-group"},
            {".gsm", "audio/x-gsm"},
            {".gtar", "application/x-gtar"},
            {".gz", "application/x-gzip"},
            {".h", "text/plain"},
            {".hdf", "application/x-hdf"},
            {".hdml", "text/x-hdml"},
            {".hhc", "application/x-oleobject"},
            {".hhk", "application/octet-stream"},
            {".hhp", "application/octet-stream"},
            {".hlp", "application/winhlp"},
            {".hpp", "text/plain"},
            {".hqx", "application/mac-binhex40"},
            {".hta", "application/hta"},
            {".htc", "text/x-component"},
            {".htm", "text/html"},
            {".html", "text/html"},
            {".htt", "text/webviewhtml"},
            {".hxa", "application/xml"},
            {".hxc", "application/xml"},
            {".hxd", "application/octet-stream"},
            {".hxe", "application/xml"},
            {".hxf", "application/xml"},
            {".hxh", "application/octet-stream"},
            {".hxi", "application/octet-stream"},
            {".hxk", "application/xml"},
            {".hxq", "application/octet-stream"},
            {".hxr", "application/octet-stream"},
            {".hxs", "application/octet-stream"},
            {".hxt", "text/html"},
            {".hxv", "application/xml"},
            {".hxw", "application/octet-stream"},
            {".hxx", "text/plain"},
            {".i", "text/plain"},
            {".ico", "image/x-icon"},
            {".ics", "application/octet-stream"},
            {".idl", "text/plain"},
            {".ief", "image/ief"},
            {".iii", "application/x-iphone"},
            {".inc", "text/plain"},
            {".inf", "application/octet-stream"},
            {".inl", "text/plain"},
            {".ins", "application/x-internet-signup"},
            {".ipa", "application/x-itunes-ipa"},
            {".ipg", "application/x-itunes-ipg"},
            {".ipproj", "text/plain"},
            {".ipsw", "application/x-itunes-ipsw"},
            {".iqy", "text/x-ms-iqy"},
            {".isp", "application/x-internet-signup"},
            {".ite", "application/x-itunes-ite"},
            {".itlp", "application/x-itunes-itlp"},
            {".itms", "application/x-itunes-itms"},
            {".itpc", "application/x-itunes-itpc"},
            {".IVF", "video/x-ivf"},
            {".jar", "application/java-archive"},
            {".java", "application/octet-stream"},
            {".jck", "application/liquidmotion"},
            {".jcz", "application/liquidmotion"},
            {".jfif", "image/pjpeg"},
            {".jnlp", "application/x-java-jnlp-file"},
            {".jpb", "application/octet-stream"},
            {".jpe", "image/jpeg"},
            {".jpeg", "image/jpeg"},
            {".jpg", "image/jpeg"},
            {".js", "application/x-javascript"},
            {".json", "application/json"},
            {".jsx", "text/jscript"},
            {".jsxbin", "text/plain"},
            {".latex", "application/x-latex"},
            {".library-ms", "application/windows-library+xml"},
            {".lit", "application/x-ms-reader"},
            {".loadtest", "application/xml"},
            {".lpk", "application/octet-stream"},
            {".lsf", "video/x-la-asf"},
            {".lst", "text/plain"},
            {".lsx", "video/x-la-asf"},
            {".lzh", "application/octet-stream"},
            {".m13", "application/x-msmediaview"},
            {".m14", "application/x-msmediaview"},
            {".m1v", "video/mpeg"},
            {".m2t", "video/vnd.dlna.mpeg-tts"},
            {".m2ts", "video/vnd.dlna.mpeg-tts"},
            {".m2v", "video/mpeg"},
            {".m3u", "audio/x-mpegurl"},
            {".m3u8", "audio/x-mpegurl"},
            {".m4a", "audio/m4a"},
            {".m4b", "audio/m4b"},
            {".m4p", "audio/m4p"},
            {".m4r", "audio/x-m4r"},
            {".m4v", "video/x-m4v"},
            {".mac", "image/x-macpaint"},
            {".mak", "text/plain"},
            {".man", "application/x-troff-man"},
            {".manifest", "application/x-ms-manifest"},
            {".map", "text/plain"},
            {".master", "application/xml"},
            {".mda", "application/msaccess"},
            {".mdb", "application/x-msaccess"},
            {".mde", "application/msaccess"},
            {".mdp", "application/octet-stream"},
            {".me", "application/x-troff-me"},
            {".mfp", "application/x-shockwave-flash"},
            {".mht", "message/rfc822"},
            {".mhtml", "message/rfc822"},
            {".mid", "audio/mid"},
            {".midi", "audio/mid"},
            {".mix", "application/octet-stream"},
            {".mk", "text/plain"},
            {".mmf", "application/x-smaf"},
            {".mno", "text/xml"},
            {".mny", "application/x-msmoney"},
            {".mod", "video/mpeg"},
            {".mov", "video/quicktime"},
            {".movie", "video/x-sgi-movie"},
            {".mp2", "video/mpeg"},
            {".mp2v", "video/mpeg"},
            {".mp3", "audio/mpeg"},
            {".mp4", "video/mp4"},
            {".mp4v", "video/mp4"},
            {".mpa", "video/mpeg"},
            {".mpe", "video/mpeg"},
            {".mpeg", "video/mpeg"},
            {".mpf", "application/vnd.ms-mediapackage"},
            {".mpg", "video/mpeg"},
            {".mpp", "application/vnd.ms-project"},
            {".mpv2", "video/mpeg"},
            {".mqv", "video/quicktime"},
            {".ms", "application/x-troff-ms"},
            {".msi", "application/octet-stream"},
            {".mso", "application/octet-stream"},
            {".mts", "video/vnd.dlna.mpeg-tts"},
            {".mtx", "application/xml"},
            {".mvb", "application/x-msmediaview"},
            {".mvc", "application/x-miva-compiled"},
            {".mxp", "application/x-mmxp"},
            {".nc", "application/x-netcdf"},
            {".nsc", "video/x-ms-asf"},
            {".nws", "message/rfc822"},
            {".ocx", "application/octet-stream"},
            {".oda", "application/oda"},
            {".odc", "text/x-ms-odc"},
            {".odh", "text/plain"},
            {".odl", "text/plain"},
            {".odp", "application/vnd.oasis.opendocument.presentation"},
            {".ods", "application/oleobject"},
            {".odt", "application/vnd.oasis.opendocument.text"},
            {".one", "application/onenote"},
            {".onea", "application/onenote"},
            {".onepkg", "application/onenote"},
            {".onetmp", "application/onenote"},
            {".onetoc", "application/onenote"},
            {".onetoc2", "application/onenote"},
            {".orderedtest", "application/xml"},
            {".osdx", "application/opensearchdescription+xml"},
            {".p10", "application/pkcs10"},
            {".p12", "application/x-pkcs12"},
            {".p7b", "application/x-pkcs7-certificates"},
            {".p7c", "application/pkcs7-mime"},
            {".p7m", "application/pkcs7-mime"},
            {".p7r", "application/x-pkcs7-certreqresp"},
            {".p7s", "application/pkcs7-signature"},
            {".pbm", "image/x-portable-bitmap"},
            {".pcast", "application/x-podcast"},
            {".pct", "image/pict"},
            {".pcx", "application/octet-stream"},
            {".pcz", "application/octet-stream"},
            {".pdf", "application/pdf"},
            {".pfb", "application/octet-stream"},
            {".pfm", "application/octet-stream"},
            {".pfx", "application/x-pkcs12"},
            {".pgm", "image/x-portable-graymap"},
            {".pic", "image/pict"},
            {".pict", "image/pict"},
            {".pkgdef", "text/plain"},
            {".pkgundef", "text/plain"},
            {".pko", "application/vnd.ms-pki.pko"},
            {".pls", "audio/scpls"},
            {".pma", "application/x-perfmon"},
            {".pmc", "application/x-perfmon"},
            {".pml", "application/x-perfmon"},
            {".pmr", "application/x-perfmon"},
            {".pmw", "application/x-perfmon"},
            {".png", "image/png"},
            {".pnm", "image/x-portable-anymap"},
            {".pnt", "image/x-macpaint"},
            {".pntg", "image/x-macpaint"},
            {".pnz", "image/png"},
            {".pot", "application/vnd.ms-powerpoint"},
            {".potm", "application/vnd.ms-powerpoint.template.macroEnabled.12"},
            {".potx", "application/vnd.openxmlformats-officedocument.presentationml.template"},
            {".ppa", "application/vnd.ms-powerpoint"},
            {".ppam", "application/vnd.ms-powerpoint.addin.macroEnabled.12"},
            {".ppm", "image/x-portable-pixmap"},
            {".pps", "application/vnd.ms-powerpoint"},
            {".ppsm", "application/vnd.ms-powerpoint.slideshow.macroEnabled.12"},
            {".ppsx", "application/vnd.openxmlformats-officedocument.presentationml.slideshow"},
            {".ppt", "application/vnd.ms-powerpoint"},
            {".pptm", "application/vnd.ms-powerpoint.presentation.macroEnabled.12"},
            {".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation"},
            {".prf", "application/pics-rules"},
            {".prm", "application/octet-stream"},
            {".prx", "application/octet-stream"},
            {".ps", "application/postscript"},
            {".psc1", "application/PowerShell"},
            {".psd", "application/octet-stream"},
            {".psess", "application/xml"},
            {".psm", "application/octet-stream"},
            {".psp", "application/octet-stream"},
            {".pub", "application/x-mspublisher"},
            {".pwz", "application/vnd.ms-powerpoint"},
            {".qht", "text/x-html-insertion"},
            {".qhtm", "text/x-html-insertion"},
            {".qt", "video/quicktime"},
            {".qti", "image/x-quicktime"},
            {".qtif", "image/x-quicktime"},
            {".qtl", "application/x-quicktimeplayer"},
            {".qxd", "application/octet-stream"},
            {".ra", "audio/x-pn-realaudio"},
            {".ram", "audio/x-pn-realaudio"},
            {".rar", "application/octet-stream"},
            {".ras", "image/x-cmu-raster"},
            {".rat", "application/rat-file"},
            {".rc", "text/plain"},
            {".rc2", "text/plain"},
            {".rct", "text/plain"},
            {".rdlc", "application/xml"},
            {".resx", "application/xml"},
            {".rf", "image/vnd.rn-realflash"},
            {".rgb", "image/x-rgb"},
            {".rgs", "text/plain"},
            {".rm", "application/vnd.rn-realmedia"},
            {".rmi", "audio/mid"},
            {".rmp", "application/vnd.rn-rn_music_package"},
            {".roff", "application/x-troff"},
            {".rpm", "audio/x-pn-realaudio-plugin"},
            {".rqy", "text/x-ms-rqy"},
            {".rtf", "application/rtf"},
            {".rtx", "text/richtext"},
            {".ruleset", "application/xml"},
            {".s", "text/plain"},
            {".safariextz", "application/x-safari-safariextz"},
            {".scd", "application/x-msschedule"},
            {".sct", "text/scriptlet"},
            {".sd2", "audio/x-sd2"},
            {".sdp", "application/sdp"},
            {".sea", "application/octet-stream"},
            {".searchConnector-ms", "application/windows-search-connector+xml"},
            {".setpay", "application/set-payment-initiation"},
            {".setreg", "application/set-registration-initiation"},
            {".settings", "application/xml"},
            {".sgimb", "application/x-sgimb"},
            {".sgml", "text/sgml"},
            {".sh", "application/x-sh"},
            {".shar", "application/x-shar"},
            {".shtml", "text/html"},
            {".sit", "application/x-stuffit"},
            {".sitemap", "application/xml"},
            {".skin", "application/xml"},
            {".sldm", "application/vnd.ms-powerpoint.slide.macroEnabled.12"},
            {".sldx", "application/vnd.openxmlformats-officedocument.presentationml.slide"},
            {".slk", "application/vnd.ms-excel"},
            {".sln", "text/plain"},
            {".slupkg-ms", "application/x-ms-license"},
            {".smd", "audio/x-smd"},
            {".smi", "application/octet-stream"},
            {".smx", "audio/x-smd"},
            {".smz", "audio/x-smd"},
            {".snd", "audio/basic"},
            {".snippet", "application/xml"},
            {".snp", "application/octet-stream"},
            {".sol", "text/plain"},
            {".sor", "text/plain"},
            {".spc", "application/x-pkcs7-certificates"},
            {".spl", "application/futuresplash"},
            {".src", "application/x-wais-source"},
            {".srf", "text/plain"},
            {".SSISDeploymentManifest", "text/xml"},
            {".ssm", "application/streamingmedia"},
            {".sst", "application/vnd.ms-pki.certstore"},
            {".stl", "application/vnd.ms-pki.stl"},
            {".sv4cpio", "application/x-sv4cpio"},
            {".sv4crc", "application/x-sv4crc"},
            {".svc", "application/xml"},
            {".swf", "application/x-shockwave-flash"},
            {".t", "application/x-troff"},
            {".tar", "application/x-tar"},
            {".tcl", "application/x-tcl"},
            {".testrunconfig", "application/xml"},
            {".testsettings", "application/xml"},
            {".tex", "application/x-tex"},
            {".texi", "application/x-texinfo"},
            {".texinfo", "application/x-texinfo"},
            {".tgz", "application/x-compressed"},
            {".thmx", "application/vnd.ms-officetheme"},
            {".thn", "application/octet-stream"},
            {".tif", "image/tiff"},
            {".tiff", "image/tiff"},
            {".tlh", "text/plain"},
            {".tli", "text/plain"},
            {".toc", "application/octet-stream"},
            {".tr", "application/x-troff"},
            {".trm", "application/x-msterminal"},
            {".trx", "application/xml"},
            {".ts", "video/vnd.dlna.mpeg-tts"},
            {".tsv", "text/tab-separated-values"},
            {".ttf", "application/octet-stream"},
            {".tts", "video/vnd.dlna.mpeg-tts"},
            {".txt", "text/plain"},
            {".u32", "application/octet-stream"},
            {".uls", "text/iuls"},
            {".user", "text/plain"},
            {".ustar", "application/x-ustar"},
            {".vb", "text/plain"},
            {".vbdproj", "text/plain"},
            {".vbk", "video/mpeg"},
            {".vbproj", "text/plain"},
            {".vbs", "text/vbscript"},
            {".vcf", "text/x-vcard"},
            {".vcproj", "Application/xml"},
            {".vcs", "text/plain"},
            {".vcxproj", "Application/xml"},
            {".vddproj", "text/plain"},
            {".vdp", "text/plain"},
            {".vdproj", "text/plain"},
            {".vdx", "application/vnd.ms-visio.viewer"},
            {".vml", "text/xml"},
            {".vscontent", "application/xml"},
            {".vsct", "text/xml"},
            {".vsd", "application/vnd.visio"},
            {".vsi", "application/ms-vsi"},
            {".vsix", "application/vsix"},
            {".vsixlangpack", "text/xml"},
            {".vsixmanifest", "text/xml"},
            {".vsmdi", "application/xml"},
            {".vspscc", "text/plain"},
            {".vss", "application/vnd.visio"},
            {".vsscc", "text/plain"},
            {".vssettings", "text/xml"},
            {".vssscc", "text/plain"},
            {".vst", "application/vnd.visio"},
            {".vstemplate", "text/xml"},
            {".vsto", "application/x-ms-vsto"},
            {".vsw", "application/vnd.visio"},
            {".vsx", "application/vnd.visio"},
            {".vtx", "application/vnd.visio"},
            {".wav", "audio/wav"},
            {".wave", "audio/wav"},
            {".wax", "audio/x-ms-wax"},
            {".wbk", "application/msword"},
            {".wbmp", "image/vnd.wap.wbmp"},
            {".wcm", "application/vnd.ms-works"},
            {".wdb", "application/vnd.ms-works"},
            {".wdp", "image/vnd.ms-photo"},
            {".webarchive", "application/x-safari-webarchive"},
            {".webtest", "application/xml"},
            {".wiq", "application/xml"},
            {".wiz", "application/msword"},
            {".wks", "application/vnd.ms-works"},
            {".WLMP", "application/wlmoviemaker"},
            {".wlpginstall", "application/x-wlpg-detect"},
            {".wlpginstall3", "application/x-wlpg3-detect"},
            {".wm", "video/x-ms-wm"},
            {".wma", "audio/x-ms-wma"},
            {".wmd", "application/x-ms-wmd"},
            {".wmf", "application/x-msmetafile"},
            {".wml", "text/vnd.wap.wml"},
            {".wmlc", "application/vnd.wap.wmlc"},
            {".wmls", "text/vnd.wap.wmlscript"},
            {".wmlsc", "application/vnd.wap.wmlscriptc"},
            {".wmp", "video/x-ms-wmp"},
            {".wmv", "video/x-ms-wmv"},
            {".wmx", "video/x-ms-wmx"},
            {".wmz", "application/x-ms-wmz"},
            {".wpl", "application/vnd.ms-wpl"},
            {".wps", "application/vnd.ms-works"},
            {".wri", "application/x-mswrite"},
            {".wrl", "x-world/x-vrml"},
            {".wrz", "x-world/x-vrml"},
            {".wsc", "text/scriptlet"},
            {".wsdl", "text/xml"},
            {".wvx", "video/x-ms-wvx"},
            {".x", "application/directx"},
            {".xaf", "x-world/x-vrml"},
            {".xaml", "application/xaml+xml"},
            {".xap", "application/x-silverlight-app"},
            {".xbap", "application/x-ms-xbap"},
            {".xbm", "image/x-xbitmap"},
            {".xdr", "text/plain"},
            {".xht", "application/xhtml+xml"},
            {".xhtml", "application/xhtml+xml"},
            {".xla", "application/vnd.ms-excel"},
            {".xlam", "application/vnd.ms-excel.addin.macroEnabled.12"},
            {".xlc", "application/vnd.ms-excel"},
            {".xld", "application/vnd.ms-excel"},
            {".xlk", "application/vnd.ms-excel"},
            {".xll", "application/vnd.ms-excel"},
            {".xlm", "application/vnd.ms-excel"},
            {".xls", "application/vnd.ms-excel"},
            {".xlsb", "application/vnd.ms-excel.sheet.binary.macroEnabled.12"},
            {".xlsm", "application/vnd.ms-excel.sheet.macroEnabled.12"},
            {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
            {".xlt", "application/vnd.ms-excel"},
            {".xltm", "application/vnd.ms-excel.template.macroEnabled.12"},
            {".xltx", "application/vnd.openxmlformats-officedocument.spreadsheetml.template"},
            {".xlw", "application/vnd.ms-excel"},
            {".xml", "text/xml"},
            {".xmta", "application/xml"},
            {".xof", "x-world/x-vrml"},
            {".XOML", "text/plain"},
            {".xpm", "image/x-xpixmap"},
            {".xps", "application/vnd.ms-xpsdocument"},
            {".xrm-ms", "text/xml"},
            {".xsc", "application/xml"},
            {".xsd", "text/xml"},
            {".xsf", "text/xml"},
            {".xsl", "text/xml"},
            {".xslt", "text/xml"},
            {".xsn", "application/octet-stream"},
            {".xss", "application/xml"},
            {".xtp", "application/octet-stream"},
            {".xwd", "image/x-xwindowdump"},
            {".z", "application/x-compress"},
            {".zip", "application/x-zip-compressed"}

        };
        public static string GetMimeType(string extension)
        {
            if (extension == null)
            {
                throw new ArgumentNullException("extension");
            }
            if (!extension.StartsWith("."))
            {
                extension = "." + extension;
            }
            string mime;
            return _mappings.TryGetValue(extension, out mime) ? mime : "application/octet-stream";
        }
        public static byte[] GetZipArchive(List<ZipItem> files)
        {
            byte[] archiveFile;
            using (var archiveStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
                {
                    foreach (var file in files)
                    {
                        var zipArchiveEntry = archive.CreateEntry(file.FileName, CompressionLevel.Fastest);
                        using (var zipStream = zipArchiveEntry.Open())
                            zipStream.Write(file.Content, 0, file.Content.Length);
                    }
                }

                archiveFile = archiveStream.ToArray();
            }
            return archiveFile;
        }
        public static async Task<List<StatusUploadFile>> UploadZipAndSaveFile(IFormFile file, string baseUrl, string[] extensionFile = null, string prefixFileName = null)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            List<StatusUploadFile> lstFile = new List<StatusUploadFile>();
            var contentType = file.ContentType;
            var extensionFileZip = Path.GetExtension(file.FileName);
            string[] ext = new string[] { ".zip", ".rar", ".7z" };
            if (!ext.Contains(extensionFileZip.ToLower()))
            {
                throw new InvalidDataException("Không đúng định dạng file .zip, .rar, .7z.");
            }
            using (MemoryStream targetMemoryStream = new MemoryStream())
            {
                await file.CopyToAsync(targetMemoryStream);
                using (ZipArchive destArchive = new ZipArchive(targetMemoryStream, ZipArchiveMode.Read, true))
                {
                    try
                    {
                        int indexFile = 0;
                        foreach (ZipArchiveEntry entry in destArchive.Entries)
                        {
                            indexFile += 1;
                            string extensionItem = Path.GetExtension(entry.FullName);
                            if (extensionFile!=null && !extensionFile.Contains(extensionItem.ToLower()))
                            {
                                lstFile.Add(new StatusUploadFile("", "", entry.FullName, extensionItem, indexFile, StatusUploadFileConstant.ERROR,"File không có trong danh sách định dạng được lưu."));
                                continue;
                            }
                            string filename = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Guid.NewGuid().ToString("N")+"_" + entry.FullName;
                            if (!string.IsNullOrEmpty(prefixFileName))
                            {
                                filename = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" +prefixFileName+"_"+ Guid.NewGuid().ToString("N")+"_" + entry.FullName;
                            }
                            var pathFile = Path.Combine(baseUrl, filename);
                            using (var stream = entry.Open())
                            {
                                Utilities.SaveFile(stream, pathFile);
                                var scanner = new AntiVirus.Scanner();
                                var result = scanner.ScanAndClean(pathFile);
                                if (result == AntiVirus.ScanResult.VirusFound)
                                {
                                    lstFile.Add(new StatusUploadFile("","", entry.FullName, extensionItem, indexFile, StatusUploadFileConstant.ERROR, "File bị nhiễm virus."));
                                    continue;
                                }
                                lstFile.Add(new StatusUploadFile(baseUrl, pathFile, entry.FullName, extensionItem, indexFile, StatusUploadFileConstant.SUCCESS));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string message = ex.Message;
                        if (ex is NotSupportedException)
                        {
                            message = "Kho lưu trữ zip, zar không hỗ trợ đọc.";
                        }
                        if (ex is ObjectDisposedException)
                        {
                            message = "Kho lưu trữ zip, zar đã được xử lý.";
                        }
                        if (ex is InvalidDataException)
                        {
                            message = "Kho lưu trữ zip, zar bị hỏng và không thể truy xuất các mục nhập của nó.";
                        }
                        throw new Exception(message);
                    }
                }

            }
            return lstFile;
        }
        public static List<StatusUploadFile> UploadFiles(IFormFileCollection files, string baseUrl, OptionSaveFile config, string[] extensionFile = null, string prefixFileName = null)
        {
            if (files == null)
            {
                throw new ArgumentNullException("files");
            }
            List<StatusUploadFile> lstFile = new List<StatusUploadFile>();
            var indexFile = 0;
            var dateNow = DateTime.Now;
            string yearPath = Path.Combine(baseUrl, dateNow.Year.ToString());
            string monthPath = Path.Combine(yearPath, dateNow.Month < 10? "0"+dateNow.Month.ToString(): dateNow.Month.ToString());
            string dayPath = Path.Combine(monthPath, dateNow.Day < 10? "0"+dateNow.Day.ToString(): dateNow.Day.ToString());
            bool existsYear = System.IO.Directory.Exists(yearPath);
            if (!existsYear)
                System.IO.Directory.CreateDirectory(yearPath);
            bool existsMonth = System.IO.Directory.Exists(monthPath);
            if (!existsMonth)
                System.IO.Directory.CreateDirectory(monthPath);
            bool existsDay = System.IO.Directory.Exists(dayPath);
            if (!existsDay)
                System.IO.Directory.CreateDirectory(dayPath);
            foreach (var file in files)
            {
                indexFile += 1;
                string extensionItem = Path.GetExtension(file.FileName).ToLower();
                if (extensionFile != null && !extensionFile.Contains(extensionItem))
                {
                    lstFile.Add(new StatusUploadFile("", "", file.FileName, extensionItem, indexFile, StatusUploadFileConstant.ERROR, "File không có trong danh sách định dạng được lưu."));
                    continue;
                }
                string filename = config.so_id+"_"+DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_"+ indexFile + extensionItem;
                if (!string.IsNullOrEmpty(prefixFileName))
                {
                    filename = config.so_id + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + indexFile +"_"+prefixFileName+ extensionItem;
                }
                var pathFile = Path.Combine(dayPath, filename);
                using (var stream = file.OpenReadStream())
                {
                    Utilities.SaveImageChangeSize(stream, pathFile, 1800, 100);
                    //await Utilities.SaveFileAsync(stream, pathFile);
                    if (config.is_duplicate_mini_file)
                    {

                    }
                    lstFile.Add(new StatusUploadFile(baseUrl, pathFile, file.FileName, extensionItem, indexFile, StatusUploadFileConstant.SUCCESS));
                }
                
            }
            return lstFile;
        }
        public static byte[] DowloadZipFile(List<string> listPath)
        {
            List<ZipItem> files = new List<ZipItem>();
            foreach (var path in listPath)
            {
                ZipItem zipItem = new ZipItem(Path.GetFileName(path), FileToByteArray(path));
                files.Add(zipItem);
            }
            return GetZipArchive(files);
        }

        private static readonly string[] VietNamChar = new string[]
        {
            "aAeEoOuUiIdDyY",
            "áàạảãâấầậẩẫăắằặẳẵ",
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
            "éèẹẻẽêếềệểễ",
            "ÉÈẸẺẼÊẾỀỆỂỄ",
            "óòọỏõôốồộổỗơớờợởỡ",
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
            "úùụủũưứừựửữ",
            "ÚÙỤỦŨƯỨỪỰỬỮ",
            "íìịỉĩ",
            "ÍÌỊỈĨ",
            "đ",
            "Đ",
            "ýỳỵỷỹ",
            "ÝỲỴỶỸ"
        };
        public static string ConvertVnToEng(string str)
        {
            for (int i = 1; i < VietNamChar.Length; i++)
            {
                for (int j = 0; j < VietNamChar[i].Length; j++)
                    str = str.Replace(VietNamChar[i][j], VietNamChar[0][i - 1]);
            }
            return str;
        }
        public static string GetBoundary(MediaTypeHeaderValue contentType, int lengthLimit)
        {
            var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary).Value;

            if (string.IsNullOrWhiteSpace(boundary))
            {
                throw new InvalidDataException("Missing content-type boundary.");
            }

            if (boundary.Length > lengthLimit)
            {
                throw new InvalidDataException(
                    $"Multipart boundary length limit {lengthLimit} exceeded.");
            }

            return boundary;
        }
        public static bool IsMultipartContentType(string contentType)
        {
            return !string.IsNullOrEmpty(contentType)
                   && contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
        }
        public static bool HasFormDataContentDisposition(ContentDispositionHeaderValue contentDisposition)
        {
            // Content-Disposition: form-data; name="key";
            return contentDisposition != null
                && contentDisposition.DispositionType.Equals("form-data")
                && string.IsNullOrEmpty(contentDisposition.FileName.Value)
                && string.IsNullOrEmpty(contentDisposition.FileNameStar.Value);
        }
        public static bool HasFileContentDisposition(ContentDispositionHeaderValue contentDisposition)
        {
            // Content-Disposition: form-data; name="myfile1"; filename="Misc 002.jpg"
            return contentDisposition != null
                && contentDisposition.DispositionType.Equals("form-data")
                && (!string.IsNullOrEmpty(contentDisposition.FileName.Value)
                    || !string.IsNullOrEmpty(contentDisposition.FileNameStar.Value));
        }
        public static string FormCollectionToJson(IFormCollection obj)
        {
            dynamic json = new JObject();
            if (obj.Keys.Any())
            {
                foreach (string key in obj.Keys)
                {
                    var value = obj[key][0];
                    json.Add(key, value);
                }
            }
            return JsonConvert.SerializeObject(json);
        }
        public async static Task<string> ReadTextFile(IFormFile file)
        {
            var result = new StringBuilder();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(await reader.ReadLineAsync());
            }
            return result.ToString();
        }
        public static string GetFileExtension(string base64String)
        {
            var data = base64String.Substring(0, 5);

            switch (data.ToUpper())
            {
                case "IVBOR":
                    return ".png";
                case "/9J/4":
                    return ".jpg";
                case "AAAAF":
                    return ".mp4";
                case "JVBER":
                    return ".pdf";
                case "AAABA":
                    return ".ico";
                case "UMFYI":
                    return ".rar";
                case "E1XYD":
                    return ".rtf";
                case "U1PKC":
                    return ".txt";
                case "PD94b":
                    return ".xml";
                case "MQOWM":
                case "77U/M":
                    return ".srt";
                default:
                    return string.Empty;
            }
        }
        public static object DecryptStrJsonByKey(object jsource, string jpath, string key, Formatting jf = Formatting.None)
        {
            JObject jobj = JObject.FromObject(jsource);
            JToken token = jobj.SelectToken(jpath);
            if (token==null || string.IsNullOrEmpty(token.ToString()))
            {
                return jsource;
            }
            var val = Utilities.DecryptByKey(token.ToString(), key);
            if (!string.IsNullOrEmpty(val))
                token.Replace(val);
            return jobj;
        }
        public static void ChuanHoaTyLeGiam(ref decimal tl_giam, ref decimal tien_giam, decimal tien)
        {
            if (tien_giam > tien)
                tien_giam = tien;
            if (tl_giam > 100)
                tl_giam = 100;
            if (tl_giam < 0)
                tl_giam = 0;

            if (tien_giam!=0 && tien!=0)
            {
                tl_giam = Math.Round(tien_giam * 100 / tien, 2);
                return;
            }    
            if (tl_giam != 0 && tien_giam==0 && tien != 0)
            {
                tien_giam = Math.Round(tien_giam * tl_giam / 100);
                return;
            }    
        }
        public static void CopyDirectoryGara(string sourcePath, string targetPath)
        {

            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            string newPath;
            foreach (string srcPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                newPath = srcPath.Replace(sourcePath, targetPath);
                File.Copy(srcPath, newPath, true);
            }
        }
    }
}
