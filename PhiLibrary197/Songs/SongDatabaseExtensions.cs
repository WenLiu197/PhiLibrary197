namespace PhiLibrary197.Songs;

/// <summary>
/// C# 15 extension members for <see cref="SongDatabase"/>.
/// </summary>
public static class SongDatabaseExtensions
{
	extension(SongDatabase database)
	{
		/// <summary>
		/// Gets a song by its full id, id prefix, or numeric variant id.
		/// </summary>
		/// <param name="id">Song id or prefix.</param>
		/// <returns>The matched song, or <see langword="null"/>.</returns>
		public SongEntry? this[string id] => database.GetSong(id);
	}
}
