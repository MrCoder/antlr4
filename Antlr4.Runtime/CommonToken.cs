/*
 * [The "BSD license"]
 *  Copyright (c) 2013 Terence Parr
 *  Copyright (c) 2013 Sam Harwell
 *  All rights reserved.
 *
 *  Redistribution and use in source and binary forms, with or without
 *  modification, are permitted provided that the following conditions
 *  are met:
 *
 *  1. Redistributions of source code must retain the above copyright
 *     notice, this list of conditions and the following disclaimer.
 *  2. Redistributions in binary form must reproduce the above copyright
 *     notice, this list of conditions and the following disclaimer in the
 *     documentation and/or other materials provided with the distribution.
 *  3. The name of the author may not be used to endorse or promote products
 *     derived from this software without specific prior written permission.
 *
 *  THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
 *  IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 *  OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 *  IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
 *  INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 *  NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 *  DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 *  THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 *  (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 *  THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */
using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Sharpen;

namespace Antlr4.Runtime
{
	[System.Serializable]
	public class CommonToken : IWritableToken
	{
		private const long serialVersionUID = -6708843461296520577L;

		protected internal int type;

		protected internal int line;

		protected internal int charPositionInLine = -1;

		protected internal int channel = DefaultChannel;

		protected internal Tuple<ITokenSource, ICharStream> source;

		/// <summary>We need to be able to change the text once in a while.</summary>
		/// <remarks>
		/// We need to be able to change the text once in a while.  If
		/// this is non-null, then getText should return this.  Note that
		/// start/stop are not affected by changing this.
		/// </remarks>
		protected internal string text;

		/// <summary>What token number is this from 0..n-1 tokens; &lt; 0 implies invalid index
		/// 	</summary>
		protected internal int index = -1;

		/// <summary>The char position into the input buffer where this token starts</summary>
		protected internal int start;

		/// <summary>The char position into the input buffer where this token stops</summary>
		protected internal int stop;

		public CommonToken(int type)
		{
			// set to invalid position
			// TODO: can store these in map in token stream rather than as field here
			this.type = type;
		}

		public CommonToken(Tuple<ITokenSource, ICharStream> source, int type, int channel
			, int start, int stop)
		{
			this.source = source;
			this.type = type;
			this.channel = channel;
			this.start = start;
			this.stop = stop;
			if (source.GetItem1() != null)
			{
				this.line = source.GetItem1().GetLine();
				this.charPositionInLine = source.GetItem1().GetCharPositionInLine();
			}
		}

		public CommonToken(int type, string text)
		{
			this.type = type;
			this.channel = DefaultChannel;
			this.text = text;
		}

		public CommonToken(IToken oldToken)
		{
			text = oldToken.GetText();
			type = oldToken.GetType();
			line = oldToken.GetLine();
			index = oldToken.GetTokenIndex();
			charPositionInLine = oldToken.GetCharPositionInLine();
			channel = oldToken.GetChannel();
			start = oldToken.GetStartIndex();
			stop = oldToken.GetStopIndex();
			if (oldToken is Antlr4.Runtime.CommonToken)
			{
				source = ((Antlr4.Runtime.CommonToken)oldToken).source;
			}
			else
			{
				source = Tuple.Create(oldToken.GetTokenSource(), oldToken.GetInputStream());
			}
		}

		public virtual int GetType()
		{
			return type;
		}

		public virtual void SetLine(int line)
		{
			this.line = line;
		}

		public virtual string GetText()
		{
			if (text != null)
			{
				return text;
			}
			ICharStream input = GetInputStream();
			if (input == null)
			{
				return null;
			}
			int n = input.Size();
			if (start < n && stop < n)
			{
				return input.GetText(Interval.Of(start, stop));
			}
			else
			{
				return "<EOF>";
			}
		}

		/// <summary>Override the text for this token.</summary>
		/// <remarks>
		/// Override the text for this token.  getText() will return this text
		/// rather than pulling from the buffer.  Note that this does not mean
		/// that start/stop indexes are not valid.  It means that that input
		/// was converted to a new string in the token object.
		/// </remarks>
		public virtual void SetText(string text)
		{
			this.text = text;
		}

		public virtual int GetLine()
		{
			return line;
		}

		public virtual int GetCharPositionInLine()
		{
			return charPositionInLine;
		}

		public virtual void SetCharPositionInLine(int charPositionInLine)
		{
			this.charPositionInLine = charPositionInLine;
		}

		public virtual int GetChannel()
		{
			return channel;
		}

		public virtual void SetChannel(int channel)
		{
			this.channel = channel;
		}

		public virtual void SetType(int type)
		{
			this.type = type;
		}

		public virtual int GetStartIndex()
		{
			return start;
		}

		public virtual void SetStartIndex(int start)
		{
			this.start = start;
		}

		public virtual int GetStopIndex()
		{
			return stop;
		}

		public virtual void SetStopIndex(int stop)
		{
			this.stop = stop;
		}

		public virtual int GetTokenIndex()
		{
			return index;
		}

		public virtual void SetTokenIndex(int index)
		{
			this.index = index;
		}

		public virtual ITokenSource GetTokenSource()
		{
			return source.GetItem1();
		}

		public virtual ICharStream GetInputStream()
		{
			return source.GetItem2();
		}

		public override string ToString()
		{
			string channelStr = string.Empty;
			if (channel > 0)
			{
				channelStr = ",channel=" + channel;
			}
			string txt = GetText();
			if (txt != null)
			{
				txt = txt.ReplaceAll("\n", "\\\\n");
				txt = txt.ReplaceAll("\r", "\\\\r");
				txt = txt.ReplaceAll("\t", "\\\\t");
			}
			else
			{
				txt = "<no text>";
			}
			return "[@" + GetTokenIndex() + "," + start + ":" + stop + "='" + txt + "',<" + type
				 + ">" + channelStr + "," + line + ":" + GetCharPositionInLine() + "]";
		}
	}
}
