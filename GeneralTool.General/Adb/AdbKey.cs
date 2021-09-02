using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralTool.General.Adb
{
    /// <summary>
    /// Adb Key
    /// </summary>
    public enum AdbKey
    {
        /// <summary>
        /// Unknown key code
        /// </summary>
        KEYCODE_UNKNOWN = 0,

        /// <summary>
        /// 通常位于手机显示屏下方，用作多功能功能功能键，用于选择显示屏左下角显示的软件定义功能。
        /// </summary>
        KEYCODE_SOFT_LEFT = 1,
        /// <summary>
        /// 通常位于手机显示屏下方，用作多功能显示器,用于选择显示屏右下角显示的软件定义功能的功能键。
        /// </summary>
        KEYCODE_SOFT_RIGHT = 2,
        /// <summary>
        /// Home
        /// </summary>
        KEYCODE_HOME = 3,
        /// <summary>
        /// 返回
        /// </summary>
        KEYCODE_BACK = 4,
        /// <summary>
        /// CALL
        /// </summary>
        KEYCODE_CALL = 5,
        /// <summary>
        /// ENDCALL
        /// </summary>
        KEYCODE_ENDCALL = 6,
        /// <summary>
        /// 0
        /// </summary>
        KEYCODE_0 = 7,
        /// <summary>
        /// 1
        /// </summary>
        KEYCODE_1 = 8,
        /// <summary>
        /// 2
        /// </summary>
        KEYCODE_2 = 9,
        /// <summary>
        /// 3
        /// </summary>
        KEYCODE_3 = 10,
        /// <summary>
        /// 4
        /// </summary>
        KEYCODE_4 = 11,
        /// <summary>
        /// 5
        /// </summary>
        KEYCODE_5 = 12,
        /// <summary>
        /// 6
        /// </summary>
        KEYCODE_6 = 13,
        /// <summary>
        /// 7
        /// </summary>
        KEYCODE_7 = 14,
        /// <summary>
        /// 8
        /// </summary>
        KEYCODE_8 = 15,
        /// <summary>
        /// 9
        /// </summary>
        KEYCODE_9 = 16,
        /// <summary>
        /// *号键
        /// </summary>
        KEYCODE_STAR = 17,
        /// <summary>
        /// #号键
        /// </summary>
        KEYCODE_POUND = 18,
        /// <summary>
        /// 向上方向键
        /// </summary>
        KEYCODE_DPAD_UP = 19,
        /// <summary>
        /// 向下方向键
        /// </summary>
        KEYCODE_DPAD_DOWN = 20,
        /// <summary>
        /// 向左方向键
        /// </summary>
        KEYCODE_DPAD_LEFT = 21,
        /// <summary>
        /// 向右方向键
        /// </summary>
        KEYCODE_DPAD_RIGHT = 22,
        /// <summary>
        /// 中间方向键
        /// </summary>
        KEYCODE_DPAD_CENTER = 23,
        /// <summary>
        /// 加大音量键
        /// </summary>
        KEYCODE_VOLUME_UP = 24,
        /// <summary>
        /// 减小音量键
        /// </summary>
        KEYCODE_VOLUME_DOWN = 25,
        /// <summary>
        /// Power
        /// </summary>
        KEYCODE_POWER = 26,
        /// <summary>
        /// 相机键
        /// </summary>
        KEYCODE_CAMERA = 27,
        /// <summary>
        /// 清除键
        /// </summary>
        KEYCODE_CLEAR = 28,
        /// <summary>
        /// A
        /// </summary>
        KEYCODE_A = 29,
        /// <summary>
        /// B
        /// </summary>
        KEYCODE_B = 30,
        /// <summary>
        /// C
        /// </summary>
        KEYCODE_C = 31,
        /// <summary>
        /// D
        /// </summary>
        KEYCODE_D = 32,
        /// <summary>
        /// E
        /// </summary>
        KEYCODE_E = 33,
        /// <summary>
        /// F
        /// </summary>
        KEYCODE_F = 34,
        /// <summary>
        /// G
        /// </summary>
        KEYCODE_G = 35,
        /// <summary>
        /// H
        /// </summary>
        KEYCODE_H = 36,
        /// <summary>
        /// I
        /// </summary>
        KEYCODE_I = 37,
        /// <summary>
        /// J
        /// </summary>
        KEYCODE_J = 38,
        /// <summary>
        /// K
        /// </summary>
        KEYCODE_K = 39,
        /// <summary>
        /// L
        /// </summary>
        KEYCODE_L = 40,
        /// <summary>
        /// M
        /// </summary>
        KEYCODE_M = 41,
        /// <summary>
        /// N
        /// </summary>
        KEYCODE_N = 42,
        /// <summary>
        /// O
        /// </summary>
        KEYCODE_O = 43,
        /// <summary>
        /// P
        /// </summary>
        KEYCODE_P = 44,
        /// <summary>
        /// Q
        /// </summary>
        KEYCODE_Q = 45,
        /// <summary>
        /// R
        /// </summary>
        KEYCODE_R = 46,
        /// <summary>
        /// S
        /// </summary>
        KEYCODE_S = 47,
        /// <summary>
        /// T
        /// </summary>
        KEYCODE_T = 48,
        /// <summary>
        /// U
        /// </summary>
        KEYCODE_U = 49,
        /// <summary>
        /// V
        /// </summary>
        KEYCODE_V = 50,
        /// <summary>
        /// W
        /// </summary>
        KEYCODE_W = 51,
        /// <summary>
        /// X
        /// </summary>
        KEYCODE_X = 52,
        /// <summary>
        /// Y
        /// </summary>
        KEYCODE_Y = 53,
        /// <summary>
        /// Z
        /// </summary>
        KEYCODE_Z = 54,
        /// <summary>
        /// ,号键
        /// </summary>
        KEYCODE_COMMA = 55,
        /// <summary>
        /// .号键
        /// </summary>
        KEYCODE_PERIOD = 56,
        /// <summary>
        /// 左Alt修改键
        /// </summary>
        KEYCODE_ALT_LEFT = 57,
        /// <summary>
        /// 右Alt修改键
        /// </summary>
        KEYCODE_ALT_RIGHT = 58,
        /// <summary>
        /// 左Shift修改键
        /// </summary>
        KEYCODE_SHIFT_LEFT = 59,
        /// <summary>
        /// 右Shift修改键
        /// </summary>
        KEYCODE_SHIFT_RIGHT = 60,
        /// <summary>
        /// Tab
        /// </summary>
        KEYCODE_TAB = 61,
        /// <summary>
        /// 空格键
        /// </summary>
        KEYCODE_SPACE = 62,
        /// <summary>
        /// 进入键盘的符号键
        /// </summary>
        KEYCODE_SYM = 63,
        /// <summary>
        /// 浏览器
        /// </summary>
        KEYCODE_EXPLORER = 64,
        /// <summary>
        /// 邮箱
        /// </summary>
        KEYCODE_ENVELOPE = 65,
        /// <summary>
        /// Enter
        /// </summary>
        KEYCODE_ENTER = 66,
        /// <summary>
        /// 删除
        /// </summary>
        KEYCODE_DEL = 67,
        /// <summary>
        /// 回收
        /// </summary>
        KEYCODE_GRAVE = 68,
        /// <summary>
        /// - 键
        /// </summary>
        KEYCODE_MINUS = 69,
        /// <summary>
        /// = 键
        /// </summary>
        KEYCODE_EQUALS = 70,
        /// <summary>
        /// [ 键
        /// </summary>
        KEYCODE_LEFT_BRACKET = 71,
        /// <summary>
        /// ] 键
        /// </summary>
        KEYCODE_RIGHT_BRACKET = 72,
        /// <summary>
        /// \ 键
        /// </summary>
        KEYCODE_BACKSLASH = 73,
        /// <summary>
        /// , 键
        /// </summary>
        KEYCODE_SEMICOLON = 74,
        /// <summary>
        /// ' 键
        /// </summary>
        KEYCODE_APOSTROPHE = 75,
        /// <summary>
        /// / 键
        /// </summary>
        KEYCODE_SLASH = 76,
        /// <summary>
        /// @ 键
        /// </summary>
        KEYCODE_AT = 77,
        /// <summary>
        /// 数字修改键
        /// </summary>
        KEYCODE_NUM = 78,
        /// <summary>
        /// 耳机挂钩键
        /// </summary>
        KEYCODE_HEADSETHOOK = 79,
        /// <summary>
        /// 相机焦点
        /// </summary>
        KEYCODE_FOCUS = 80,   // *Camera* focus
        /// <summary>
        /// +
        /// </summary>
        KEYCODE_PLUS = 81,
        /// <summary>
        /// 菜单
        /// </summary>
        KEYCODE_MENU = 82,
        /// <summary>
        /// 通知
        /// </summary>
        KEYCODE_NOTIFICATION = 83,
        /// <summary>
        /// 搜索
        /// </summary>
        KEYCODE_SEARCH = 84,
        /// <summary>
        /// 媒体暂停
        /// </summary>
        KEYCODE_MEDIA_PLAY_PAUSE = 85,
        /// <summary>
        /// 媒体停止
        /// </summary>
        KEYCODE_MEDIA_STOP = 86,
        /// <summary>
        /// 媒体下一个
        /// </summary>
        KEYCODE_MEDIA_NEXT = 87,
        /// <summary>
        /// 媒体上一个
        /// </summary>
        KEYCODE_MEDIA_PREVIOUS = 88,
        /// <summary>
        /// 媒体重复
        /// </summary>
        KEYCODE_MEDIA_REWIND = 89,
        /// <summary>
        /// 媒体快进
        /// </summary>
        KEYCODE_MEDIA_FAST_FORWARD = 90,
        /// <summary>
        /// 静音
        /// </summary>
        KEYCODE_MUTE = 91,
        /// <summary>
        /// Page up
        /// </summary>
        KEYCODE_PAGE_UP = 92,
        /// <summary>
        /// Page down
        /// </summary>
        KEYCODE_PAGE_DOWN = 93,
        /// <summary>
        /// 相册
        /// </summary>
        KEYCODE_PICTSYMBOLS = 94,
        /// <summary>
        /// 开关字符集修改键
        /// </summary>
        KEYCODE_SWITCH_CHARSET = 95,   // switch char-sets (Kanji,Katakana)
        /** Key code constant: A Button key.
         * On a game controller, the A button should be either the button labeled A
         * or the first button on the bottom row of controller buttons. */
        KEYCODE_BUTTON_A = 96,
        /** Key code constant: B Button key.
         * On a game controller, the B button should be either the button labeled B
         * or the second button on the bottom row of controller buttons. */
        KEYCODE_BUTTON_B = 97,
        /** Key code constant: C Button key.
         * On a game controller, the C button should be either the button labeled C
         * or the third button on the bottom row of controller buttons. */
        KEYCODE_BUTTON_C = 98,
        /** Key code constant: X Button key.
         * On a game controller, the X button should be either the button labeled X
         * or the first button on the upper row of controller buttons. */
        KEYCODE_BUTTON_X = 99,
        /** Key code constant: Y Button key.
         * On a game controller, the Y button should be either the button labeled Y
         * or the second button on the upper row of controller buttons. */
        KEYCODE_BUTTON_Y = 100,
        /** Key code constant: Z Button key.
         * On a game controller, the Z button should be either the button labeled Z
         * or the third button on the upper row of controller buttons. */
        KEYCODE_BUTTON_Z = 101,
        /** Key code constant: L1 Button key.
         * On a game controller, the L1 button should be either the button labeled L1 (or L)
         * or the top left trigger button. */
        KEYCODE_BUTTON_L1 = 102,
        /** Key code constant: R1 Button key.
         * On a game controller, the R1 button should be either the button labeled R1 (or R)
         * or the top right trigger button. */
        KEYCODE_BUTTON_R1 = 103,
        /** Key code constant: L2 Button key.
         * On a game controller, the L2 button should be either the button labeled L2
         * or the bottom left trigger button. */
        KEYCODE_BUTTON_L2 = 104,
        /** Key code constant: R2 Button key.
         * On a game controller, the R2 button should be either the button labeled R2
         * or the bottom right trigger button. */
        KEYCODE_BUTTON_R2 = 105,
        /** Key code constant: Left Thumb Button key.
         * On a game controller, the left thumb button indicates that the left (or only)
         * joystick is pressed. */
        KEYCODE_BUTTON_THUMBL = 106,
        /** Key code constant: Right Thumb Button key.
         * On a game controller, the right thumb button indicates that the right
         * joystick is pressed. */
        KEYCODE_BUTTON_THUMBR = 107,
        /** Key code constant: Start Button key.
         * On a game controller, the button labeled Start. */
        KEYCODE_BUTTON_START = 108,
        /** Key code constant: Select Button key.
         * On a game controller, the button labeled Select. */
        KEYCODE_BUTTON_SELECT = 109,
        /** Key code constant: Mode Button key.
         * On a game controller, the button labeled Mode. */
        KEYCODE_BUTTON_MODE = 110,
        /** Key code constant: Escape key. */
        KEYCODE_ESCAPE = 111,
        /** Key code constant: Forward Delete key.
         * Deletes characters ahead of the insertion point, unlike {@link #KEYCODE_DEL}. */
        KEYCODE_FORWARD_DEL = 112,
        /** Key code constant: Left Control modifier key. */
        KEYCODE_CTRL_LEFT = 113,
        /** Key code constant: Right Control modifier key. */
        KEYCODE_CTRL_RIGHT = 114,
        /** Key code constant: Caps Lock key. */
        KEYCODE_CAPS_LOCK = 115,
        /** Key code constant: Scroll Lock key. */
        KEYCODE_SCROLL_LOCK = 116,
        /** Key code constant: Left Meta modifier key. */
        KEYCODE_META_LEFT = 117,
        /** Key code constant: Right Meta modifier key. */
        KEYCODE_META_RIGHT = 118,
        /** Key code constant: Function modifier key. */
        KEYCODE_FUNCTION = 119,
        /** Key code constant: System Request / Print Screen key. */
        KEYCODE_SYSRQ = 120,
        /** Key code constant: Break / Pause key. */
        KEYCODE_BREAK = 121,
        /** Key code constant: Home Movement key.
         * Used for scrolling or moving the cursor around to the start of a line
         * or to the top of a list. */
        KEYCODE_MOVE_HOME = 122,
        /** Key code constant: End Movement key.
         * Used for scrolling or moving the cursor around to the end of a line
         * or to the bottom of a list. */
        KEYCODE_MOVE_END = 123,
        /** Key code constant: Insert key.
         * Toggles insert / overwrite edit mode. */
        KEYCODE_INSERT = 124,
        /** Key code constant: Forward key.
         * Navigates forward in the history stack.  Complement of {@link #KEYCODE_BACK}. */
        KEYCODE_FORWARD = 125,
        /** Key code constant: Play media key. */
        KEYCODE_MEDIA_PLAY = 126,
        /** Key code constant: Pause media key. */
        KEYCODE_MEDIA_PAUSE = 127,
        /** Key code constant: Close media key.
         * May be used to close a CD tray, for example. */
        KEYCODE_MEDIA_CLOSE = 128,
        /** Key code constant: Eject media key.
         * May be used to eject a CD tray, for example. */
        KEYCODE_MEDIA_EJECT = 129,
        /** Key code constant: Record media key. */
        KEYCODE_MEDIA_RECORD = 130,
        /** Key code constant: F1 key. */
        KEYCODE_F1 = 131,
        /** Key code constant: F2 key. */
        KEYCODE_F2 = 132,
        /** Key code constant: F3 key. */
        KEYCODE_F3 = 133,
        /** Key code constant: F4 key. */
        KEYCODE_F4 = 134,
        /** Key code constant: F5 key. */
        KEYCODE_F5 = 135,
        /** Key code constant: F6 key. */
        KEYCODE_F6 = 136,
        /** Key code constant: F7 key. */
        KEYCODE_F7 = 137,
        /** Key code constant: F8 key. */
        KEYCODE_F8 = 138,
        /** Key code constant: F9 key. */
        KEYCODE_F9 = 139,
        /** Key code constant: F10 key. */
        KEYCODE_F10 = 140,
        /** Key code constant: F11 key. */
        KEYCODE_F11 = 141,
        /** Key code constant: F12 key. */
        KEYCODE_F12 = 142,
        /** Key code constant: Num Lock key.
         * This is the Num Lock key, it is different from {@link #KEYCODE_NUM}.
         * This key alters the behavior of other keys on the numeric keypad. */
        KEYCODE_NUM_LOCK = 143,
        /** Key code constant: Numeric keypad '0' key. */
        KEYCODE_NUMPAD_0 = 144,
        /** Key code constant: Numeric keypad '1' key. */
        KEYCODE_NUMPAD_1 = 145,
        /** Key code constant: Numeric keypad '2' key. */
        KEYCODE_NUMPAD_2 = 146,
        /** Key code constant: Numeric keypad '3' key. */
        KEYCODE_NUMPAD_3 = 147,
        /** Key code constant: Numeric keypad '4' key. */
        KEYCODE_NUMPAD_4 = 148,
        /** Key code constant: Numeric keypad '5' key. */
        KEYCODE_NUMPAD_5 = 149,
        /** Key code constant: Numeric keypad '6' key. */
        KEYCODE_NUMPAD_6 = 150,
        /** Key code constant: Numeric keypad '7' key. */
        KEYCODE_NUMPAD_7 = 151,
        /** Key code constant: Numeric keypad '8' key. */
        KEYCODE_NUMPAD_8 = 152,
        /** Key code constant: Numeric keypad '9' key. */
        KEYCODE_NUMPAD_9 = 153,
        /** Key code constant: Numeric keypad '/' key (for division). */
        KEYCODE_NUMPAD_DIVIDE = 154,
        /** Key code constant: Numeric keypad '*' key (for multiplication). */
        KEYCODE_NUMPAD_MULTIPLY = 155,
        /** Key code constant: Numeric keypad '-' key (for subtraction). */
        KEYCODE_NUMPAD_SUBTRACT = 156,
        /** Key code constant: Numeric keypad '+' key (for addition). */
        KEYCODE_NUMPAD_ADD = 157,
        /** Key code constant: Numeric keypad '.' key (for decimals or digit grouping). */
        KEYCODE_NUMPAD_DOT = 158,
        /** Key code constant: Numeric keypad ',' key (for decimals or digit grouping). */
        KEYCODE_NUMPAD_COMMA = 159,
        /** Key code constant: Numeric keypad Enter key. */
        KEYCODE_NUMPAD_ENTER = 160,
        /** Key code constant: Numeric keypad '=' key. */
        KEYCODE_NUMPAD_EQUALS = 161,
        /** Key code constant: Numeric keypad '(' key. */
        KEYCODE_NUMPAD_LEFT_PAREN = 162,
        /** Key code constant: Numeric keypad ')' key. */
        KEYCODE_NUMPAD_RIGHT_PAREN = 163,
        /** Key code constant: Volume Mute key.
         * Mutes the speaker, unlike {@link #KEYCODE_MUTE}.
         * This key should normally be implemented as a toggle such that the first press
         * mutes the speaker and the second press restores the original volume. */
        KEYCODE_VOLUME_MUTE = 164,
        /** Key code constant: Info key.
         * Common on TV remotes to show additional information related to what is
         * currently being viewed. */
        KEYCODE_INFO = 165,
        /** Key code constant: Channel up key.
         * On TV remotes, increments the television channel. */
        KEYCODE_CHANNEL_UP = 166,
        /** Key code constant: Channel down key.
         * On TV remotes, decrements the television channel. */
        KEYCODE_CHANNEL_DOWN = 167,
        /** Key code constant: Zoom in key. */
        KEYCODE_ZOOM_IN = 168,
        /** Key code constant: Zoom out key. */
        KEYCODE_ZOOM_OUT = 169,
        /** Key code constant: TV key.
         * On TV remotes, switches to viewing live TV. */
        KEYCODE_TV = 170,
        /** Key code constant: Window key.
         * On TV remotes, toggles picture-in-picture mode or other windowing functions. */
        KEYCODE_WINDOW = 171,
        /** Key code constant: Guide key.
         * On TV remotes, shows a programming guide. */
        KEYCODE_GUIDE = 172,
        /** Key code constant: DVR key.
         * On some TV remotes, switches to a DVR mode for recorded shows. */
        KEYCODE_DVR = 173,
        /** Key code constant: Bookmark key.
         * On some TV remotes, bookmarks content or web pages. */
        KEYCODE_BOOKMARK = 174,
        /** Key code constant: Toggle captions key.
         * Switches the mode for closed-captioning text, for example during television shows. */
        KEYCODE_CAPTIONS = 175,
        /** Key code constant: Settings key.
         * Starts the system settings activity. */
        KEYCODE_SETTINGS = 176,
        /** Key code constant: TV power key.
         * On TV remotes, toggles the power on a television screen. */
        KEYCODE_TV_POWER = 177,
        /** Key code constant: TV input key.
         * On TV remotes, switches the input on a television screen. */
        KEYCODE_TV_INPUT = 178,
        /** Key code constant: Set-top-box power key.
         * On TV remotes, toggles the power on an external Set-top-box. */
        KEYCODE_STB_POWER = 179,
        /** Key code constant: Set-top-box input key.
         * On TV remotes, switches the input mode on an external Set-top-box. */
        KEYCODE_STB_INPUT = 180,
        /** Key code constant: A/V Receiver power key.
         * On TV remotes, toggles the power on an external A/V Receiver. */
        KEYCODE_AVR_POWER = 181,
        /** Key code constant: A/V Receiver input key.
         * On TV remotes, switches the input mode on an external A/V Receiver. */
        KEYCODE_AVR_INPUT = 182,
        /** Key code constant: Red "programmable" key.
         * On TV remotes, acts as a contextual/programmable key. */
        KEYCODE_PROG_RED = 183,
        /** Key code constant: Green "programmable" key.
         * On TV remotes, actsas a contextual/programmable key. */
        KEYCODE_PROG_GREEN = 184,
        /** Key code constant: Yellow "programmable" key.
         * On TV remotes, acts as a contextual/programmable key. */
        KEYCODE_PROG_YELLOW = 185,
        /** Key code constant: Blue "programmable" key.
         * On TV remotes, acts as a contextual/programmable key. */
        KEYCODE_PROG_BLUE = 186,
        /** Key code constant: App switch key.
         * Should bring up the application switcher dialog. */
        KEYCODE_APP_SWITCH = 187,
        /** Key code constant: Generic Game Pad Button #1.*/
        KEYCODE_BUTTON_1 = 188,
        /** Key code constant: Generic Game Pad Button #2.*/
        KEYCODE_BUTTON_2 = 189,
        /** Key code constant: Generic Game Pad Button #3.*/
        KEYCODE_BUTTON_3 = 190,
        /** Key code constant: Generic Game Pad Button #4.*/
        KEYCODE_BUTTON_4 = 191,
        /** Key code constant: Generic Game Pad Button #5.*/
        KEYCODE_BUTTON_5 = 192,
        /** Key code constant: Generic Game Pad Button #6.*/
        KEYCODE_BUTTON_6 = 193,
        /** Key code constant: Generic Game Pad Button #7.*/
        KEYCODE_BUTTON_7 = 194,
        /** Key code constant: Generic Game Pad Button #8.*/
        KEYCODE_BUTTON_8 = 195,
        /** Key code constant: Generic Game Pad Button #9.*/
        KEYCODE_BUTTON_9 = 196,
        /** Key code constant: Generic Game Pad Button #10.*/
        KEYCODE_BUTTON_10 = 197,
        /** Key code constant: Generic Game Pad Button #11.*/
        KEYCODE_BUTTON_11 = 198,
        /** Key code constant: Generic Game Pad Button #12.*/
        KEYCODE_BUTTON_12 = 199,
        /** Key code constant: Generic Game Pad Button #13.*/
        KEYCODE_BUTTON_13 = 200,
        /** Key code constant: Generic Game Pad Button #14.*/
        KEYCODE_BUTTON_14 = 201,
        /** Key code constant: Generic Game Pad Button #15.*/
        KEYCODE_BUTTON_15 = 202,
        /** Key code constant: Generic Game Pad Button #16.*/
        KEYCODE_BUTTON_16 = 203,
        /** Key code constant: Language Switch key.
         * Toggles the current input language such as switching between English and Japanese on
         * a QWERTY keyboard.  On some devices, the same function may be performed by
         * pressing Shift+Spacebar. */
        KEYCODE_LANGUAGE_SWITCH = 204,
        /** Key code constant: Manner Mode key.
         * Toggles silent or vibrate mode on and off to make the device behave more politely
         * in certain settings such as on a crowded train.  On some devices, the key may only
         * operate when long-pressed. */
        KEYCODE_MANNER_MODE = 205,
        /** Key code constant: 3D Mode key.
         * Toggles the display between 2D and 3D mode. */
        KEYCODE_3D_MODE = 206,
        /** Key code constant: Contacts special function key.
         * Used to launch an address book application. */
        KEYCODE_CONTACTS = 207,
        /** Key code constant: Calendar special function key.
         * Used to launch a calendar application. */
        KEYCODE_CALENDAR = 208,
        /** Key code constant: Music special function key.
         * Used to launch a music player application. */
        KEYCODE_MUSIC = 209,
        /** Key code constant: Calculator special function key.
         * Used to launch a calculator application. */
        KEYCODE_CALCULATOR = 210,
        /** Key code constant: Japanese full-width / half-width key. */
        KEYCODE_ZENKAKU_HANKAKU = 211,
        /** Key code constant: Japanese alphanumeric key. */
        KEYCODE_EISU = 212,
        /** Key code constant: Japanese non-conversion key. */
        KEYCODE_MUHENKAN = 213,
        /** Key code constant: Japanese conversion key. */
        KEYCODE_HENKAN = 214,
        /** Key code constant: Japanese katakana / hiragana key. */
        KEYCODE_KATAKANA_HIRAGANA = 215,
        /** Key code constant: Japanese Yen key. */
        KEYCODE_YEN = 216,
        /** Key code constant: Japanese Ro key. */
        KEYCODE_RO = 217,
        /** Key code constant: Japanese kana key. */
        KEYCODE_KANA = 218,
        /** Key code constant: Assist key.
         * Launches the global assist activity.  Not delivered to applications. */
        KEYCODE_ASSIST = 219,
        /** Key code constant: Brightness Down key.
         * Adjusts the screen brightness down. */
        KEYCODE_BRIGHTNESS_DOWN = 220,
        /** Key code constant: Brightness Up key.
         * Adjusts the screen brightness up. */
        KEYCODE_BRIGHTNESS_UP = 221,
        /** Key code constant: Audio Track key.
         * Switches the audio tracks. */
        KEYCODE_MEDIA_AUDIO_TRACK = 222,
        /** Key code constant: Sleep key.
         * Puts the device to sleep.  Behaves somewhat like {@link #KEYCODE_POWER} but it
         * has no effect if the device is already asleep. */
        KEYCODE_SLEEP = 223,
        /** Key code constant: Wakeup key.
         * Wakes up the device.  Behaves somewhat like {@link #KEYCODE_POWER} but it
         * has no effect if the device is already awake. */
        KEYCODE_WAKEUP = 224,
        /** Key code constant: Pairing key.
         * Initiates peripheral pairing mode. Useful for pairing remote control
         * devices or game controllers, especially if no other input mode is
         * available. */
        KEYCODE_PAIRING = 225,
        /** Key code constant: Media Top Menu key.
         * Goes to the top of media menu. */
        KEYCODE_MEDIA_TOP_MENU = 226,
        /** Key code constant: '11' key. */
        KEYCODE_11 = 227,
        /** Key code constant: '12' key. */
        KEYCODE_12 = 228,
        /** Key code constant: Last Channel key.
         * Goes to the last viewed channel. */
        KEYCODE_LAST_CHANNEL = 229,
        /** Key code constant: TV data service key.
         * Displays data services like weather, sports. */
        KEYCODE_TV_DATA_SERVICE = 230,
        /** Key code constant: Voice Assist key.
         * Launches the global voice assist activity. Not delivered to applications. */
        KEYCODE_VOICE_ASSIST = 231,
        /** Key code constant: Radio key.
         * Toggles TV service / Radio service. */
        KEYCODE_TV_RADIO_SERVICE = 232,
        /** Key code constant: Teletext key.
         * Displays Teletext service. */
        KEYCODE_TV_TELETEXT = 233,
        /** Key code constant: Number entry key.
         * Initiates to enter multi-digit channel nubmber when each digit key is assigned
         * for selecting separate channel. Corresponds to Number Entry Mode (0x1D) of CEC
         * User Control Code. */
        KEYCODE_TV_NUMBER_ENTRY = 234,
        /** Key code constant: Analog Terrestrial key.
         * Switches to analog terrestrial broadcast service. */
        KEYCODE_TV_TERRESTRIAL_ANALOG = 235,
        /** Key code constant: Digital Terrestrial key.
         * Switches to digital terrestrial broadcast service. */
        KEYCODE_TV_TERRESTRIAL_DIGITAL = 236,
        /** Key code constant: Satellite key.
         * Switches to digital satellite broadcast service. */
        KEYCODE_TV_SATELLITE = 237,
        /** Key code constant: BS key.
         * Switches to BS digital satellite broadcasting service available in Japan. */
        KEYCODE_TV_SATELLITE_BS = 238,
        /** Key code constant: CS key.
         * Switches to CS digital satellite broadcasting service available in Japan. */
        KEYCODE_TV_SATELLITE_CS = 239,
        /** Key code constant: BS/CS key.
         * Toggles between BS and CS digital satellite services. */
        KEYCODE_TV_SATELLITE_SERVICE = 240,
        /** Key code constant: Toggle Network key.
         * Toggles selecting broacast services. */
        KEYCODE_TV_NETWORK = 241,
        /** Key code constant: Antenna/Cable key.
         * Toggles broadcast input source between antenna and cable. */
        KEYCODE_TV_ANTENNA_CABLE = 242,
        /** Key code constant: HDMI #1 key.
         * Switches to HDMI input #1. */
        KEYCODE_TV_INPUT_HDMI_1 = 243,
        /** Key code constant: HDMI #2 key.
         * Switches to HDMI input #2. */
        KEYCODE_TV_INPUT_HDMI_2 = 244,
        /** Key code constant: HDMI #3 key.
         * Switches to HDMI input #3. */
        KEYCODE_TV_INPUT_HDMI_3 = 245,
        /** Key code constant: HDMI #4 key.
         * Switches to HDMI input #4. */
        KEYCODE_TV_INPUT_HDMI_4 = 246,
        /** Key code constant: Composite #1 key.
         * Switches to composite video input #1. */
        KEYCODE_TV_INPUT_COMPOSITE_1 = 247,
        /** Key code constant: Composite #2 key.
         * Switches to composite video input #2. */
        KEYCODE_TV_INPUT_COMPOSITE_2 = 248,
        /** Key code constant: Component #1 key.
         * Switches to component video input #1. */
        KEYCODE_TV_INPUT_COMPONENT_1 = 249,
        /** Key code constant: Component #2 key.
         * Switches to component video input #2. */
        KEYCODE_TV_INPUT_COMPONENT_2 = 250,
        /** Key code constant: VGA #1 key.
         * Switches to VGA (analog RGB) input #1. */
        KEYCODE_TV_INPUT_VGA_1 = 251,
        /** Key code constant: Audio description key.
         * Toggles audio description off / on. */
        KEYCODE_TV_AUDIO_DESCRIPTION = 252,
        /** Key code constant: Audio description mixing volume up key.
         * Louden audio description volume as compared with normal audio volume. */
        KEYCODE_TV_AUDIO_DESCRIPTION_MIX_UP = 253,
        /** Key code constant: Audio description mixing volume down key.
         * Lessen audio description volume as compared with normal audio volume. */
        KEYCODE_TV_AUDIO_DESCRIPTION_MIX_DOWN = 254,
        /** Key code constant: Zoom mode key.
         * Changes Zoom mode (Normal, Full, Zoom, Wide-zoom, etc.) */
        KEYCODE_TV_ZOOM_MODE = 255,
        /** Key code constant: Contents menu key.
         * Goes to the title list. Corresponds to Contents Menu (0x0B) of CEC User Control
         * Code */
        KEYCODE_TV_CONTENTS_MENU = 256,
        /** Key code constant: Media context menu key.
         * Goes to the context menu of media contents. Corresponds to Media Context-sensitive
         * Menu (0x11) of CEC User Control Code. */
        KEYCODE_TV_MEDIA_CONTEXT_MENU = 257,
        /** Key code constant: Timer programming key.
         * Goes to the timer recording menu. Corresponds to Timer Programming (0x54) of
         * CEC User Control Code. */
        KEYCODE_TV_TIMER_PROGRAMMING = 258,
        /** Key code constant: Help key. */
        KEYCODE_HELP = 259,
        /** Key code constant: Navigate to previous key.
         * Goes backward by one item in an ordered collection of items. */
        KEYCODE_NAVIGATE_PREVIOUS = 260,
        /** Key code constant: Navigate to next key.
         * Advances to the next item in an ordered collection of items. */
        KEYCODE_NAVIGATE_NEXT = 261,
        /** Key code constant: Navigate in key.
         * Activates the item that currently has focus or expands to the next level of a navigation
         * hierarchy. */
        KEYCODE_NAVIGATE_IN = 262,
        /** Key code constant: Navigate out key.
         * Backs out one level of a navigation hierarchy or collapses the item that currently has
         * focus. */
        KEYCODE_NAVIGATE_OUT = 263,
        /** Key code constant: Skip forward media key. */
        KEYCODE_MEDIA_SKIP_FORWARD = 272,
        /** Key code constant: Skip backward media key. */
        KEYCODE_MEDIA_SKIP_BACKWARD = 273,
        /** Key code constant: Step forward media key.
         * Steps media forward, one frame at a time. */
        KEYCODE_MEDIA_STEP_FORWARD = 274,
        /** Key code constant: Step backward media key.
         * Steps media backward, one frame at a time. */
        KEYCODE_MEDIA_STEP_BACKWARD = 275

    }
}
