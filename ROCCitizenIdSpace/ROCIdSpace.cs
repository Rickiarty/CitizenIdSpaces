using System.Collections.Immutable;

namespace ROCCitizenIdSpace
{
	/// <summary>
	/// a space to possibility to ids of citizens in R.O.C.
	/// </summary>
	public static class ROCIdSpace
	{
		/// <summary>
		/// English alphabet -> corresponding integer value 
		/// </summary>
		public static readonly ImmutableDictionary<string, int> A_to_I = new Dictionary<string, int>()
		{
			{"A", 10}, {"B", 11}, {"C", 12}, {"D", 13}, {"E", 14}, {"F", 15}, {"G", 16}, {"H", 17}, {"I", 34}, {"J", 18},
			{"K", 19}, {"L", 20}, {"M", 21}, {"N", 22}, {"O", 35}, {"P", 23}, {"Q", 24}, {"R", 25}, {"S", 26}, {"T", 27},
			{"U", 28}, {"V", 29}, {"W", 32}, {"X", 30}, {"Y", 31}, {"Z", 33},
		}.ToImmutableDictionary();
		/// <summary>
		/// English alphabet -> 行政區 
		/// </summary>
		public static readonly ImmutableDictionary<string, string> A_to_District = new Dictionary<string, string>()
		{
			{"A", "台北市"}, {"B", "台中市"}, {"C", "基隆市"}, {"D", "台南市"},
			{"E", "高雄市"}, {"F", "新北市"}, {"G", "宜蘭縣"}, {"H", "桃園市"},
			{"I", "嘉義市"}, {"J", "新竹縣"}, {"K", "苗栗縣"}, {"L", "台中縣"},
			{"M", "南投縣"}, {"N", "彰化縣"}, {"O", "新竹市"}, {"P", "雲林縣"},
			{"Q", "嘉義縣"}, {"R", "台南縣"}, {"S", "高雄縣"}, {"T", "屏東縣"},
			{"U", "花蓮縣"}, {"V", "台東縣"}, {"W", "金門縣"}, {"X", "澎湖縣"},
			{"Y", "陽明山管理局"}, {"Z", "連江縣"},
		}.ToImmutableDictionary();
		/// <summary>
		/// digit order -> weight(權重) 
		/// </summary>
		public static readonly ImmutableDictionary<int, int> Weight = new Dictionary<int, int>()
		{
			{0, 1}, {1, 9},
			{2, 8}, {3, 7},
			{4, 6}, {5, 5}, {6, 4}, {7, 3}, {8, 2}, {9, 1},
			{10,1},
		}.ToImmutableDictionary();

		/// <summary>
		/// 可能的 身分證字號 之 清單 (by lazy evaluation) 
		/// </summary>
		public static IEnumerable<string> PossibleIds
		{
			get
			{
				int range = (int)Math.Pow(10, 7);
				int[] digits = new int[11];
				foreach (KeyValuePair<string, int> kvpair in A_to_I)
				{
					digits[0] = kvpair.Value / 10;
					digits[1] = kvpair.Value % 10;
					for (int sn = 0; sn < range; sn += 1)
					{
						int n1 = (int)Math.Pow(10, 6);
						for (int i = 0; i < 7; i += 1)
						{
							digits[i + 3] = (sn / n1) % 10;
							n1 /= 10;
						}
						string possibleId = kvpair.Key;
						int sum = 0;
						#region male
						digits[2] = 1; // male 
						digits[10] = 0;
						sum = Weight.Sum(kvpair => kvpair.Value * digits[kvpair.Key]);
						digits[10] = (10 - (sum % 10)) % 10;
						for (int k = 2; k < 11; k += 1)
						{
							possibleId += digits[k].ToString();
						}
						#endregion male
						yield return possibleId;
						possibleId = kvpair.Key;
						#region female
						digits[2] = 2; // female 
						digits[10] = 0;
						sum = Weight.Sum(kvpair => kvpair.Value * digits[kvpair.Key]);
						digits[10] = (10 - (sum % 10)) % 10;
						for (int k = 2; k < 11; k += 1)
						{
							possibleId += digits[k].ToString();
						}
						#endregion female
						yield return possibleId;
					}
				}
			}
		}

		/// <summary>
		/// 驗證ID有效性
		/// </summary>
		/// <param name="roc_id">身分證字號</param>
		/// <returns>是否有效</returns>
		public static bool VerifyId(string roc_id)
		{
			if (string.IsNullOrWhiteSpace(roc_id)) return false;
			string prefix = roc_id[0].ToString();
			if (!A_to_I.Any(e => e.Key == prefix)) return false;
			int v = A_to_I[prefix];
			int[] digits = new int[11];
			digits[0] = v / 10;
			digits[1] = v % 10;
			for (int i = 1; i < roc_id.Length; i += 1)
			{
				int digit = 0;
				if (int.TryParse(roc_id[i].ToString(), out digit))
				{
					digits[i + 1] = digit;
				}
				else
				{
					return false;
				}
			}
			int sum = Weight.Sum(kvpair => kvpair.Value * digits[kvpair.Key]);
			return sum % 10 == 0;
		}

	}
}
