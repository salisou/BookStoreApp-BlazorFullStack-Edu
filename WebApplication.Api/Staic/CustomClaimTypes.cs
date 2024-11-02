namespace WebApplicationApi.Staic
{
	/// <summary>
	/// Classe statica per definire i tipi di claim personalizzati.
	/// Utilizzata per aggiungere tipi di claim univoci che non fanno parte del set standard.
	/// </summary>
	public static class CustomClaimTypes
	{
		/// <summary>
		/// Identificatore univoco per l'utente, 
		/// usato per differenziarlo dagli altri tipi di claim standard.
		/// </summary>
		public const string Uid = "uid";
	}
}