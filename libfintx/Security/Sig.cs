﻿/*	
 * 	
 *  This file is part of libfintx.
 *  
 *  Copyright (c) 2017 Torsten Klinger
 * 	E-Mail: torsten.klinger@googlemail.com
 * 	
 * 	libfintx is free software; you can redistribute it and/or
 *	modify it under the terms of the GNU Lesser General Public
 * 	License as published by the Free Software Foundation; either
 * 	version 2.1 of the License, or (at your option) any later version.
 *	
 * 	libfintx is distributed in the hope that it will be useful,
 * 	but WITHOUT ANY WARRANTY; without even the implied warranty of
 * 	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * 	Lesser General Public License for more details.
 *	
 * 	You should have received a copy of the GNU Lesser General Public
 * 	License along with libfintx; if not, write to the Free Software
 * 	Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA
 * 	
 */

using System;
using System.Security.Cryptography;
using System.Text;

namespace libfintx
{
    public class Sig
    {
        public static string SECFUNC_HBCI_SIG_RDH = "1";
        public static string SECFUNC_HBCI_SIG_DDV = "2";

        public static string SECFUNC_FINTS_SIG_DIG = "1";
        public static string SECFUNC_FINTS_SIG_SIG = "2";

        public static string SECFUNC_SIG_PT_1STEP = "999";
        public static string SECFUNC_SIG_PT_2STEP_MIN = "900";
        public static string SECFUNC_SIG_PT_2STEP_MAX = "997";

        public static string HASHALG_SHA1 = "1";
        public static string HASHALG_SHA256 = "3";
        public static string HASHALG_SHA384 = "4";
        public static string HASHALG_SHA512 = "5";
        public static string HASHALG_SHA256_SHA256 = "6";
        public static string HASHALG_RIPEMD160 = "999";

        public static string SIGALG_DES = "1";
        public static string SIGALG_RSA = "10";

        public static string SIGMODE_ISO9796_1 = "16";
        public static string SIGMODE_ISO9796_2 = "17";
        public static string SIGMODE_PKCS1 = "18";
        public static string SIGMODE_PSS = "19";
        public static string SIGMODE_RETAIL_MAC = "999";


        public static byte[] SignDataSHA256 (string Message)
        {
            var message = Encoding.Default.GetBytes(Message);

            SHA256Managed hashString = new SHA256Managed();
            
            var hashValue = hashString.ComputeHash(message);

            if (DEBUG.Enabled)
                DEBUG.Write("Hashed message: " + libfintx.Converter.ByteArrayToString(hashValue));

            return hashValue;
        }

        public static byte[] SignMessage(byte[] hash)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(RDH_KEYSTORE.KEY_SIGNING_PRIVATE_XML);

                var signedMessage = rsa.SignHash(hash, CryptoConfig.MapNameToOID("SHA256"));

                if (DEBUG.Enabled)
                    DEBUG.Write("Signed message: " + libfintx.Converter.ByteArrayToString(signedMessage));

                return signedMessage;
            }
        }

        public static bool Verify(byte[] hash, byte[] signature)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(RDH_KEYSTORE.KEY_SIGNING_PRIVATE_XML);

                if (!rsa.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA256"), signature))
                    throw new CryptographicException();
                else
                    return true;
            }
        }
    }
}
