using Chess.Base.PGN;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Chess.Base.Tests
{
	[TestFixture]
	public class TestPGNParser2
	{
		static string GameData =
@"

[Event ""URS-qualJ""]
[Site ""Sochi""]
[Date ""1990.??.??""]
[Round ""?""]
[White ""Kramnik, Vladimir""]
[Black ""Tiviakov, Sergei""]
[Result ""1-0""]
[ECO ""E32""]
[WhiteElo ""2450""]
[BlackElo ""2500""]
[PlyCount ""79""]
[EventDate ""1990.??.??""]
[Source ""ChessBase""]
[SourceDate ""1996.11.15""]

1. d4 {E32: Nimzo-Indian: Classical (4 Qc2): 4...0-0} 1... Nf6 2. c4 e6 3. Nc3
Bb4 4. Qc2 O-O 5. a3 Bxc3+ 6. Qxc3 b6 7. Bg5 Bb7 8. Nh3 d6 9. f3 Nbd7 10. e4 e5
11. d5 a5 12. b3 h6 13. Be3 c5 14. Nf2 Nh5 15. g3 {Controls f4} 15... Bc8 16.
Be2 Ra7 17. Qd2 Qf6 18. a4 Re8 19. O-O-O Nf8 20. Kb1 Ng6 21. Rhf1 Qe7 22. Bd3
Qf6 23. Rde1 Nf8 24. f4 exf4 25. gxf4 Ng6 (25... Qh4 $5 $11 {is interesting})
26. f5 $16 26... Ne5 27. Be2 Qh4 28. Bxh6 $1 {Demolition of pawn structure}
28... Ng3 (28... gxh6 29. Qxh6) 29. hxg3 Qxh6 30. Qf4 g5 (30... Nd7 31. Rh1
Qxf4 32. gxf4 $16) 31. fxg6 (31. Qd2 31... Qg7 $18) 31... Qxg6 (31... Qxf4 32.
gxf4 Nxg6 33. f5 $16) 32. Rh1 $18 32... Kg7 (32... Qg7 33. g4 $18) 33. Rh5 f6
34. Reh1 Nf7 35. R5h4 35... Re5 $2 (35... Qg5 $18) 36. Nd3 (36. Bh5 {
and White can already relax} 36... Rxh5 37. Rxh5 37... Re7 $18) 36... Rg5 37.
Bh5 Rxh5 38. Rxh5 Re7 39. Rh7+ Kf8 (39... Kg8 {is not much help} 40. Nf2 $18)
40. Qh4 (40. Qh4 Rxe4 41. g4 $14) (40. Rh8+ {and White has reached his goal}
40... Nxh8 41. Rxh8+ Kf7 42. Rxc8 Qxe4 43. Qxe4 Rxe4 44. Rb8 $18) 1-0";

		[Test]
		public void TestParser1()
		{
			var parser = new PGNParser();
			parser.ParsePGN(GameData);
		}

		[Test]
		public void TestParser2()
		{
			var data = File.ReadAllText("..\\..\\..\\TestData\\annotatedsetone.pgn");
			var parser = new PGNParser();
			parser.ParsePGN(data);
		}

		[Test]
		public void TestParser3()
		{
			var data = File.ReadAllText("..\\..\\..\\TestData\\perle.pgn");
			var parser = new PGNParser();
			parser.ParsePGN(data);
		}

		[Test]
		public void TestMainVariation()
		{
			var parser = new PGNParser();
			var g = parser.ParsePGN(GameData);
			var game = g.Games[0];
			var variation = game.GetMainVariation();
			Assert.IsTrue(variation.All(x => x is PGNMove));
			Assert.IsTrue(variation.Count > 0);
		}

		[Test]
		public void TestGameEncoder()
		{
			var parser = new PGNParser();
			var g = parser.ParsePGN(GameData);
			var game = g.Games[0];
			var enc = GameEncoder.EncodeGame(game);
		}
	}
}
