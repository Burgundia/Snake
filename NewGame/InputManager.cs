using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewGame
{
    class InputManager
    {
        private static Dictionary<Keys, bool> keyTable = new Dictionary<Keys, bool>();

        public static bool KeyPressed(Keys key)
        {
            if (!keyTable.ContainsKey(key))
            {
                return false;
            }

            return (bool)keyTable[key];
        }

        public static void ChangeState(Keys key, bool state)
        {
            keyTable[key] = state;
        }
    }
}
