/*
* Copyright 2007 ZXing authors
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*      http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.Text.RegularExpressions;

namespace ZXing.Client.Result
{
   /// <summary> Tries to parse results that are a URI of some kind.
   /// 
   /// </summary>
   /// <author>  Sean Owen
   /// </author>
   /// <author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source 
   /// </author>
   sealed class URIResultParser : ResultParser
   {
      private const String PATTERN_END =
          "(:\\d{1,5})?" + // maybe port
          "(/|\\?|$)"; // query, path or nothing
      private static readonly Regex URL_WITH_PROTOCOL_PATTERN = new Regex(
          "[a-zA-Z0-9]{2,}:(/)*" + // protocol
          "[a-zA-Z0-9\\-]+(\\.[a-zA-Z0-9\\-]+)*" + // host name elements
          PATTERN_END
#if !(SILVERLIGHT4)
         , RegexOptions.Compiled);
#else
         );
#endif

      private static readonly Regex URL_WITHOUT_PROTOCOL_PATTERN = new Regex(
          "([a-zA-Z0-9\\-]+\\.)+[a-zA-Z0-9\\-]{2,}" + // host name elements
          PATTERN_END
#if !(SILVERLIGHT4)
         , RegexOptions.Compiled);
#else
         );
#endif

      override public ParsedResult parse(ZXing.Result result)
      {
         String rawText = result.Text;
         // We specifically handle the odd "URL" scheme here for simplicity
         if (rawText.StartsWith("URL:"))
         {

            rawText = rawText.Substring(4);
         }
         rawText = rawText.Trim();
         return isBasicallyValidURI(rawText) ? new URIParsedResult(rawText, null) : null;
      }

      internal static bool isBasicallyValidURI(String uri)
      {
         var m = URL_WITH_PROTOCOL_PATTERN.Match(uri);
         if (m.Success && m.Index == 0)
         { // match at start only
            return true;
         }
         m = URL_WITHOUT_PROTOCOL_PATTERN.Match(uri);
         return m.Success && m.Index == 0;
      }
   }
}