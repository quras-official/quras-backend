using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quras
{
    public class OpCode
    {
        public static byte PUSHBYTES20 = 0x14;
        public static byte PUSHBYTES8 = 0x08;
        public static byte PUSHBYTES75 = 0x4B;
        public static byte PUSHDATA1 = 0x4C; // The next byte contains the number of bytes to be pushed onto the stack.
        public static byte PUSHDATA2 = 0x4D; // The next two bytes contain the number of bytes to be pushed onto the stack.
        public static byte PUSHDATA4 = 0x4E; // The next four bytes contain the number of bytes to be pushed onto the stack.

        public static byte PUSH1 = 0x51; // The number 1 is pushed onto the stack.
        public static byte PUSHT = PUSH1;
        public static byte PUSH2 = 0x52; // The number 2 is pushed onto the stack.
        public static byte PUSH3 = 0x53; // The number 3 is pushed onto the stack.
        public static byte PUSH4 = 0x54; // The number 4 is pushed onto the stack.
        public static byte PUSH5 = 0x55; // The number 5 is pushed onto the stack.
        public static byte PUSH6 = 0x56; // The number 6 is pushed onto the stack.
        public static byte PUSH7 = 0x57; // The number 7 is pushed onto the stack.
        public static byte PUSH8 = 0x58; // The number 8 is pushed onto the stack.
        public static byte PUSH9 = 0x59; // The number 9 is pushed onto the stack.
        public static byte PUSH10 = 0x5A; // The number 10 is pushed onto the stack.
        public static byte PUSH11 = 0x5B; // The number 11 is pushed onto the stack.
        public static byte PUSH12 = 0x5C; // The number 12 is pushed onto the stack.
        public static byte PUSH13 = 0x5D; // The number 13 is pushed onto the stack.
        public static byte PUSH14 = 0x5E; // The number 14 is pushed onto the stack.
        public static byte PUSH15 = 0x5F; // The number 15 is pushed onto the stack.
        public static byte PUSH16 = 0x60; // The number 16 is pushed onto the stack.

        public static byte SYSCALL = 0x68;

        public static byte PUSHM1 = 0x4F; // The number -1 is pushed onto the stack.
        public static byte PUSH0 = 0x00; // An empty array of bytes is pushed onto the stack.
    }
}
