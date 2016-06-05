﻿using System.Collections.Generic;
using System.Text.RegularExpressions;

using Rant.Internal.Stringes;

namespace Rant.Internal.Engine.Syntax
{
	/// <summary>
	/// Replaces text in a pattern output according to a regular expression and evaluator pattern.
	/// </summary>
	internal class RAReplacer : RantAction
	{
		private readonly RantAction _sourceAction;
		private readonly RantAction _matchEvalAction;
		private readonly Regex _regex;

		public RAReplacer(Stringe range, Regex regex, RantAction sourceAction, RantAction matchEvalAction) : base(range)
		{
			_regex = regex;
			_sourceAction = sourceAction;
			_matchEvalAction = matchEvalAction;
		}

		public override IEnumerator<RantAction> Run(Sandbox sb)
		{
			sb.AddOutputWriter();
			yield return _sourceAction;
			var input = sb.Return().Main;
			var matches = _regex.Matches(input);
			int start = 0;
			foreach (Match match in matches)
			{
				sb.RegexMatches.Push(match);
				sb.AddOutputWriter();
				yield return _matchEvalAction;
				var result = sb.Return().Main;
                sb.Print(input.Substring(start, match.Index - start));
				sb.Print(result);
                sb.RegexMatches.Pop();
				start = match.Index + match.Length;
			}
			sb.Print(input.Substring(start, input.Length - start));
		}
	}
}