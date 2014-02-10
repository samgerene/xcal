﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reexmonkey.xcal.domain.extensions
{
    public static class EncodingExtensions
    {
        /// <summary>
        /// Converts plain text to its equivalent encoded Base64 string
        /// </summary>
        /// <param name="unicode">The plain text (unencoded) that is to be encoded</param>
        /// <returns>The Base64 string encoded from the plain text</returns>
        /// <exception cref="ArgumentNullException">Thrown when the plain text argument is null</exception>
        /// <exception cref="EncoderFallbackException">Throw when encoding the plain text to Base64 fails</exception>
        public static string EncodeToBase64(this string unicode)
        {
            string base64 = string.Empty;
            try
            {
                var bytes = Encoding.Unicode.GetBytes(unicode);
                base64 = Convert.ToBase64String(bytes);
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException(ex.ToString(), ex);
            }
            catch (EncoderFallbackException ex)
            {
                throw new EncoderFallbackException(ex.ToString(), ex);
            }
            return base64;
        }

        public static string EncodeToUtf8(this string unicode)
        {
            var encoded = string.Empty;
            try
            {
                var utf8 = new UTF8Encoding();
                var bytes = utf8.GetBytes(unicode);
                encoded = utf8.GetString(bytes);
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException(ex.ToString(), ex);
            }
            catch (EncoderFallbackException ex)
            {
                throw new EncoderFallbackException(ex.ToString(), ex);
            }
            return encoded;
        }

        /// <summary>
        /// Converts plain text to its equivalent encoded Base64-based raw binary
        /// </summary>
        /// <param name="unicode">The unicode to be byte-encode</param>
        /// <returns>The Base64-based binary value encoded from the plain text</returns>
        /// <exception cref="ArgumentNullException">Throw when the plain text argument is null</exception>
        /// <exception cref="EncoderFallbackException">Throw when encoding the plain text to Base64 fails</exception>
        public static IEnumerable<byte> ToBytes(this string unicode)
        {
            IEnumerable<byte> bytes = null;
            if (unicode == null) throw new ArgumentNullException();
            try
            {
                bytes = Encoding.Unicode.GetBytes(unicode);
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentException(ex.ToString(), ex);
            }
            catch (EncoderFallbackException ex)
            {
                throw new EncoderFallbackException(ex.ToString(), ex);
            }
            return bytes;
        }

        /// <summary>
        /// Converts a Base64 string to its equivalent encoded plain text
        /// </summary>
        /// <param name="base64">The base64 text (encoded) that is to be decoded</param>
        /// <returns>Plain text decoded from the Base64 text</returns>
        /// <exception cref="ArgumentNullException">Thrown when the plain text argument is null</exception>
        /// <exception cref="ArgumentException">Thrown when conversion from Base64 to raw binary data fails</exception>
        /// <exception cref="FormatException">Thrown when conversion from Base64 to raw binary data fails</exception>
        /// <exception cref="DecoderFallbackException">Thrown when decoding from raw binary data to plain text fails</exception>
        public static string DecodeToUnicode(this string base64)
        {
            var unicode = string.Empty;
            try
            {
                var bytes = Convert.FromBase64String(base64);
                unicode = Encoding.Unicode.GetString(bytes);
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException(ex.ToString(), ex);
            }
            catch (FormatException ex)
            {
                throw new ArgumentException(ex.ToString(), ex);
            }
            catch (DecoderFallbackException ex)
            {
                throw new DecoderFallbackException(ex.ToString(), ex);
            }
            return unicode;

        }
    }
}
