﻿using System;
using System.Collections.Generic;
using System.IO;
using GOLD;
using HueLib2;
using Action = HueLib2.Action;

//Generated by the GOLD Parser Builder

namespace WinHue3
{
    public class WinHueParser
    {
        private Parser parser = new Parser();
        public ParseError Error { get; set; }

        private enum SymbolIndex
        {
            @Eof = 0,                                  // (EOF)
            @Error = 1,                                // (Error)
            @Whitespace = 2,                           // Whitespace
            @Lparen = 3,                               // '('
            @Rparen = 4,                               // ')'
            @Minusgt = 5,                              // '->'
            @Bool = 6,                                 // bool
            @Bri = 7,                                  // bri
            @Ct = 8,                                   // ct
            @Float = 9,                                // float
            @Group = 10,                               // group
            @Hue = 11,                                 // hue
            @Light = 12,                               // light
            @Loop = 13,                                // loop
            @Number = 14,                              // number
            @On = 15,                                  // on
            @Sat = 16,                                 // sat
            @Sc = 17,                                  // SC
            @Tt = 18,                                  // tt
            @Wait = 19,                                // wait
            @X = 20,                                   // x
            @Y = 21,                                   // y
            @Loopval = 22,                             // <loopval>
            @Object = 23,                              // <object>
            @Properties = 24,                          // <properties>
            @Property = 25,                            // <property>
            @Sc2 = 26,                                 // <sc>
            @Statement = 27,                           // <statement>
            @Statements = 28                           // <statements>
        }

        private enum ProductionIndex
        {
            @Loopval_Number = 0,                       // <loopval> ::= number
            @Loopval = 1,                              // <loopval> ::= 
            @Sc_Sc = 2,                                // <sc> ::= SC <sc>
            @Sc_Sc2 = 3,                               // <sc> ::= SC
            @Object_Light_Number = 4,                  // <object> ::= light number
            @Object_Group_Number = 5,                  // <object> ::= group number
            @Statements = 6,                           // <statements> ::= <statement> <statements>
            @Statements2 = 7,                          // <statements> ::= <statement>
            @Statement_Minusgt = 8,                    // <statement> ::= <object> '->' <properties> <sc>
            @Statement_Wait_Number = 9,                // <statement> ::= wait number <sc>
            @Statement_Loop_Lparen_Rparen = 10,        // <statement> ::= loop <loopval> '(' <statements> ')' <sc>
            @Properties = 11,                          // <properties> ::= <property> <properties>
            @Properties2 = 12,                         // <properties> ::= <property>
            @Property_Hue_Number = 13,                 // <property> ::= hue number
            @Property_Bri_Number = 14,                 // <property> ::= bri number
            @Property_Sat_Number = 15,                 // <property> ::= sat number
            @Property_Tt_Number = 16,                  // <property> ::= tt number
            @Property_X_Float_Y_Float = 17,            // <property> ::= x float y float
            @Property_Ct_Number = 18,                  // <property> ::= ct number
            @Property_On_Bool = 19                     // <property> ::= on bool
        }

        public object program;     //You might derive a specific object

        public bool Parse(string script)
        {
            //This procedure starts the GOLD Parser Engine and handles each of the
            //messages it returns. Each time a reduction is made, you can create new
            //custom object and reassign the .CurrentReduction property. Otherwise, 
            //the system will use the Reduction object that was returned.
            //
            //The resulting tree will be a pure representation of the language 
            //and will be ready to implement.

            ParseMessage response;
            bool done;                      //Controls when we leave the loop
            bool accepted = false;          //Was the parse successful?

            TextReader reader = new StringReader(script);

            parser.Open(reader);
            parser.TrimReductions = false;  //Please read about this feature before enabling  

            done = false;

            List<AnimatorCommand> listcommands = new List<AnimatorCommand>();
            AnimatorCommand currentCommand;

            while (!done)
            {
                response = parser.Parse();

                switch (response)
                {
                    case ParseMessage.LexicalError:
                        //Cannot recognize token
                        done = true;
                        Error = new ParseError() { linenumber = parser.CurrentPosition().Line, charpos = parser.CurrentPosition().Column, message = $"token not recognized : '{parser.CurrentToken().Data}' at line {parser.CurrentPosition().Line} col {parser.CurrentPosition().Column}" };
                        break;

                    case ParseMessage.SyntaxError:
                        //Expecting a different token
                        done = true;
                        Error = new ParseError() { linenumber = parser.CurrentPosition().Line, charpos = parser.CurrentPosition().Column, message = $"Expecting another token : '{parser.CurrentToken().Data}' at line {parser.CurrentPosition().Line} col {parser.CurrentPosition().Column}" };
                        break;

                    case ParseMessage.Reduction:
                        //Create a customized object to store the reduction

                        parser.CurrentReduction = CreateNewObject(parser.CurrentReduction as Reduction);
                        break;

                    case ParseMessage.Accept:
                        //Accepted!
                        //program = parser.CurrentReduction   //The root node!                 
                        done = true;
                        accepted = true;
                        program = parser.CurrentReduction;
                        break;

                    case ParseMessage.TokenRead:
                        //You don't have to do anything here.
                        break;

                    case ParseMessage.InternalError:
                        //INTERNAL ERROR! Something is horribly wrong.
                        done = true;
                        break;

                    case ParseMessage.NotLoadedError:
                        //This error occurs if the CGT was not loaded.                   
                        done = true;
                        break;

                    case ParseMessage.GroupError:
                        //GROUP ERROR! Unexpected end of file
                        done = true;
                        break;
                }
            } //while

            return accepted;
        }

        private object CreateNewObject(Reduction r)
        {
            object result = null;

            switch ((ProductionIndex)r.Parent.TableIndex())
            {
                case ProductionIndex.Loopval_Number:
                    // <loopval> ::= number
                    result = new AnimatorCommandLoop() {loopleft = (int) r.get_Data(1)};

                    break;

                case ProductionIndex.Loopval:
                    // <loopval> ::= 
                    result = new AnimatorCommandLoop() {loopleft = -1};
                    break;

                case ProductionIndex.Sc_Sc:
                    // <sc> ::= SC <sc>
                    break;

                case ProductionIndex.Sc_Sc2:
                    // <sc> ::= SC
                    result = -1;
                    break;

                case ProductionIndex.Object_Light_Number:
                    // <object> ::= light number
                    result = new AnimatorCommandSet()
                    {
                        objset = new Light {Id = r.get_Data(1).ToString(), state = new State()}
                    };
                    break;

                case ProductionIndex.Object_Group_Number:
                    // <object> ::= group number
                    result = new AnimatorCommandSet()
                    {
                        objset = new Group {Id = r.get_Data(1).ToString(), action = new Action()}
                    };
                    break;

                case ProductionIndex.Statements:
                    // <statements> ::= <statement> <statements>
                    break;

                case ProductionIndex.Statements2:
                    // <statements> ::= <statement>
                    break;

                case ProductionIndex.Statement_Minusgt:
                    // <statement> ::= <object> '->' <properties> <sc>
                    AnimatorCommandSet ac = (AnimatorCommandSet)r.get_Data(0);


                    if (ac.objset.HasProperty("state"))
                    {
                         ((Light) ac.objset).state = new State(GetProperties(r.get_Data(2) as Reduction));
                    }
                    else
                    {
                        ((Group) ac.objset).action = new Action(GetProperties(r.get_Data(2) as Reduction));
                    }
                    break;

                case ProductionIndex.Statement_Wait_Number:
                    // <statement> ::= wait number <sc>
                    result = new AnimatorCommandWait() {waitamount = Convert.ToInt32(r.get_Data(2))};
                    break;

                case ProductionIndex.Statement_Loop_Lparen_Rparen:
                    // <statement> ::= loop <loopval> '(' <statements> ')' <sc>
                    break;

                case ProductionIndex.Properties:
                    // <properties> ::= <property> <properties>
                    result = r;
                    break;

                case ProductionIndex.Properties2:
                    // <properties> ::= <property>
                    result = r;
                    break;

                case ProductionIndex.Property_Hue_Number:
                    // <property> ::= hue number
                    try
                    {
                        result = new CommonProperties() { hue = Convert.ToUInt16(r.get_Data(1).ToString()) };
                    }
                    catch (Exception)
                    {
                        Error = new ParseError() { linenumber = parser.CurrentPosition().Line, charpos = parser.CurrentPosition().Column, message = "Invalid value : expected ushort (0-65525) " };
                    }
                    
                    break;

                case ProductionIndex.Property_Bri_Number:
                    // <property> ::= bri number
                    try
                    {
                        result = new CommonProperties() { bri = Convert.ToByte(r.get_Data(1)) };
                    }
                    catch (Exception)
                    {
                        Error = new ParseError() { linenumber = parser.CurrentPosition().Line, charpos = parser.CurrentPosition().Column, message = "Invalid value : expected byte (0-255) " };                       
                    }
                    
                    break;

                case ProductionIndex.Property_Sat_Number:
                    // <property> ::= sat number
                    try
                    {
                        result = new CommonProperties() { sat = Convert.ToByte(r.get_Data(1)) };
                    }
                    catch (Exception)
                    {
                        Error = new ParseError() { linenumber = parser.CurrentPosition().Line, charpos = parser.CurrentPosition().Column, message = "Invalid value : expected byte (0-255) " };
                    }
                    
                    break;

                case ProductionIndex.Property_Tt_Number:
                    // <property> ::= tt number
                    try
                    {
                        result = new CommonProperties() { transitiontime = Convert.ToUInt16(r.get_Data(1)) };
                    }
                    catch (Exception)
                    {
                        Error = new ParseError() { linenumber = parser.CurrentPosition().Line, charpos = parser.CurrentPosition().Column, message = "Invalid value : expected ushort (0-65525) " };
                    }
                    
                    break;

                case ProductionIndex.Property_X_Float_Y_Float:
                    // <property> ::= x float y float
                    try
                    {
                        result = new CommonProperties()
                        {
                            xy = new XY() { x = Convert.ToDecimal(r.get_Data(1)), y = Convert.ToDecimal(r.get_Data(3)) }
                        };
                        if(!(((CommonProperties)result).xy.x <= (decimal)1.000 && ((CommonProperties)result).xy.x >= (decimal)0.000 && ((CommonProperties)result).xy.y <= (decimal)1.000 && ((CommonProperties)result).xy.y >= (decimal)0.000))
                        {
                            Error = new ParseError() { linenumber = parser.CurrentPosition().Line, charpos = parser.CurrentPosition().Column, message = "Invalid value : expected decimal between 0.000 and 1.000 " };
                        }
                    }
                    catch (Exception)
                    {
                        Error = new ParseError()
                        {
                            linenumber = parser.CurrentPosition().Line,
                            charpos = parser.CurrentPosition().Column,
                            message = "Invalid value : expected decimal between 0.000 and 1.000 "
                        };

                    }
                        
                    break;

                case ProductionIndex.Property_Ct_Number:
                    // <property> ::= ct number
                    try
                    {
                        result = new CommonProperties() { ct = Convert.ToUInt16(r.get_Data(1).ToString()) };
                        if(!(((CommonProperties)result).ct >=153 && ((CommonProperties)result).ct <= 500))
                        {
                            Error = new ParseError() { linenumber = parser.CurrentPosition().Line, charpos = parser.CurrentPosition().Column, message = "Invalid value : expected between 153 and 500 " };
                        }
                    }
                    catch (Exception)
                    {
                        Error = new ParseError() { linenumber = parser.CurrentPosition().Line, charpos = parser.CurrentPosition().Column, message = "Invalid value : expected between 153 and 500 " };

                    }
                    
                    break;

                case ProductionIndex.Property_On_Bool:
                    // <property> ::= on bool
                    try
                    {
                        result = new CommonProperties() { on = Convert.ToBoolean(r.get_Data(1)) };
                    }
                    catch (Exception)
                    {
                        Error = new ParseError() { linenumber = parser.CurrentPosition().Line, charpos = parser.CurrentPosition().Column, message = "Invalid value : expected true or false " };

                    }
                    
                    break;

            }  //switch

            return result;
        }

        private CommonProperties GetProperties(Reduction r)
        {
            CommonProperties prop = new CommonProperties();

            Stack<Reduction> listred = new Stack<Reduction>();

            listred.Push(r);

            while (listred.Count > 0)
            {
                Reduction red = listred.Pop();

                for (int i = 0; i < red.Count(); i++)
                {
                    if (red[i].Parent.TableIndex() == (short) SymbolIndex.Property)
                    {
                        prop += (CommonProperties) red[i].Data;
                    }
                    else
                    {
                        listred.Push((Reduction)red[i].Data);
                    }
                }

            }

            return prop;
        }



        public WinHueParser()
        {
            var type = typeof(ResourceLoader);
            var fullName = type.Namespace + "." + "Interpreter.WH3A.egt";
            var stream = type.Assembly.GetManifestResourceStream(fullName);
            BinaryReader br = new BinaryReader(stream);
            parser.LoadTables(br);
        }

    } //MyParser

    public class ParseError
    {
        public string message;
        public int linenumber;
        public int charpos;
    }

}
