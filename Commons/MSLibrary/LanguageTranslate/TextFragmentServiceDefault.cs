using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.LanguageTranslate
{
    public class TextFragmentServiceDefault : ITextFragmentService
    {
        public async Task<string> GetDefaultText(TextFragment fragment)
        {
            return await Task.FromResult(GetDefaultTextSync(fragment));
        }

        public string GetDefaultTextSync(TextFragment fragment)
        {
            List<string> parameters = new List<string>();

            foreach(var item in fragment.ReplaceParameters)
            {
                if (item is TextFragment)
                {
                    parameters.Add(GetDefaultTextSync((TextFragment)item));
                }
                else
                {
                    parameters.Add(item.ToString());
                }
            }

            return string.Format(fragment.DefaultFormatting, parameters.ToArray());
        }

        public async Task<string> GetLanguageText(int lcid, TextFragment fragment)
        {
            return await Task.FromResult(GetLanguageTextSync(lcid,fragment));
        }

        public string GetLanguageTextSync(int lcid, TextFragment fragment)
        {
            var formatting = StringLanguageTranslate.Translate(fragment.Code, lcid, fragment.DefaultFormatting);
            List<string> parameters = new List<string>();
            foreach(var item in fragment.ReplaceParameters)
            {
                if (item is TextFragment)
                {
                    parameters.Add(GetLanguageTextSync(lcid, (TextFragment)item));
                }
                else
                {
                    parameters.Add(item.ToString());
                }
            }

            return string.Format(formatting,parameters.ToArray());
        }
    }

    public class TextFragmentServiceDefalutFactory : SingletonFactory<ITextFragmentService>
    {
        protected override ITextFragmentService RealCreate()
        {
            TextFragmentServiceDefault textFragmentServiceDefault = new TextFragmentServiceDefault();
            return textFragmentServiceDefault;
        }
    }
}
