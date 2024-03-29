﻿/*
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
using Manufaktura.Controls.Model;
using Manufaktura.Controls.Services;
using System;
using System.Diagnostics;

namespace Manufaktura.Controls.Rendering
{
    /// <summary>
    /// Strategy of drawing specific musical symbol.
    /// </summary>
    /// <typeparam name="TElement">Musical symbol type</typeparam>
    public abstract class MusicalSymbolRenderStrategy<TElement> : MusicalSymbolRenderStrategyBase where TElement : MusicalSymbol
    {
        protected readonly IScoreService scoreService;

        protected MusicalSymbolRenderStrategy(IScoreService scoreService)
        {
            this.scoreService = scoreService;
        }

        /// <summary>
        /// Musica symbol whose rendering is supported by this strategy
        /// </summary>
		public override Type SymbolType { get { return typeof(TElement); } }

        /// <summary>
        /// Draw musical symbol
        /// </summary>
        /// <param name="element">Element to draw</param>
        /// <param name="renderer">Renderer</param>
        public abstract void Render(TElement element, ScoreRendererBase renderer);

        /// <summary>
        /// Draw musical symbol
        /// </summary>
        /// <param name="element">Element to draw</param>
        /// <param name="renderer">Renderer</param>
        public override void Render(MusicalSymbol element, ScoreRendererBase renderer)
        {
            if (element.IsBreakpointSet) Debugger.Break();
            var startPositionX = scoreService.CursorPositionX;
            Render((TElement)element, renderer);
            element.RenderedWidth = scoreService.CursorPositionX - startPositionX;
        }
    }
}