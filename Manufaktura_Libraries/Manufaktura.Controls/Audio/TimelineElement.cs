/*
 * Copyright 2018 Manufaktura Programów Jacek Salamon http://musicengravingcontrols.com/
 * MIT LICENCE
 
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
using System;

namespace Manufaktura.Controls.Audio
{
    /// <summary>
    /// Class that indicates that a specific element has to be played at specific time
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
	public class TimelineElement<TElement>
    {
        /// <summary>
        /// Initializes a new instance of TimelineElement which indicates that element "what" has to be played at specific time "when".
        /// </summary>
        /// <param name="when"></param>
        /// <param name="what"></param>
		public TimelineElement(TimeSpan when, TElement what)
        {
            When = when;
            What = what;
        }

        /// <summary>
        /// Element to be played
        /// </summary>
		public TElement What { get; private set; }

        /// <summary>
        /// Time of playing element
        /// </summary>
		public TimeSpan When { get; private set; }
    }
}