using System.Text.Json.Serialization;

namespace FuriaChatBotApi.Model {
    /// <summary>
    /// Modelo de Partidas obtidas pela API do Panda Score
    /// </summary>
    public class Match {
        [JsonPropertyName("draw")]
        public bool? Draw { get; set; }

        [JsonPropertyName("videogame")]
        public Videogame? Videogame { get; set; }

        [JsonPropertyName("serie_id")]
        public int? SerieId { get; set; }

        [JsonPropertyName("winner")]
        public Team? Winner { get; set; }

        [JsonPropertyName("detailed_stats")]
        public bool? DetailedStats { get; set; }

        [JsonPropertyName("winner_id")]
        public int? WinnerId { get; set; }

        [JsonPropertyName("tournament_id")]
        public int? TournamentId { get; set; }

        [JsonPropertyName("match_type")]
        public string? MatchType { get; set; }

        [JsonPropertyName("streams_list")]
        public List<StreamInfo>? StreamsList { get; set; }

        [JsonPropertyName("serie")]
        public Serie? Serie { get; set; }

        [JsonPropertyName("slug")]
        public string? Slug { get; set; }

        [JsonPropertyName("scheduled_at")]
        public string? ScheduledAt { get; set; }

        [JsonPropertyName("live")]
        public LiveInfo? Live { get; set; }

        [JsonPropertyName("game_advantage")]
        public object? GameAdvantage { get; set; }

        [JsonPropertyName("end_at")]
        public string? EndAt { get; set; }

        [JsonPropertyName("games")]
        public List<Game>? Games { get; set; }

        [JsonPropertyName("opponents")]
        public List<OpponentWrapper>? Opponents { get; set; }

        [JsonPropertyName("videogame_version")]
        public VideogameVersion? VideogameVersion { get; set; }

        [JsonPropertyName("rescheduled")]
        public bool? Rescheduled { get; set; }

        [JsonPropertyName("league_id")]
        public int? LeagueId { get; set; }

        [JsonPropertyName("number_of_games")]
        public int? NumberOfGames { get; set; }

        [JsonPropertyName("modified_at")]
        public string? ModifiedAt { get; set; }

        [JsonPropertyName("begin_at")]
        public string? BeginAt { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("tournament")]
        public Tournament? Tournament { get; set; }

        [JsonPropertyName("videogame_title")]
        public object? VideogameTitle { get; set; }

        [JsonPropertyName("league")]
        public League? League { get; set; }

        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("winner_type")]
        public string? WinnerType { get; set; }

        [JsonPropertyName("forfeit")]
        public bool? Forfeit { get; set; }

        [JsonPropertyName("original_scheduled_at")]
        public string? OriginalScheduledAt { get; set; }

        [JsonPropertyName("results")]
        public List<Result>? Results { get; set; }
    }

    public class Videogame {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("slug")]
        public string? Slug { get; set; }
    }

    public class Team {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("location")]
        public string? Location { get; set; }

        [JsonPropertyName("slug")]
        public string? Slug { get; set; }

        [JsonPropertyName("modified_at")]
        public string? ModifiedAt { get; set; }

        [JsonPropertyName("acronym")]
        public string? Acronym { get; set; }

        [JsonPropertyName("image_url")]
        public string? ImageUrl { get; set; }
    }

    public class StreamInfo {
        [JsonPropertyName("main")]
        public bool? Main { get; set; }

        [JsonPropertyName("language")]
        public string? Language { get; set; }

        [JsonPropertyName("embed_url")]
        public string? EmbedUrl { get; set; }

        [JsonPropertyName("official")]
        public bool? Official { get; set; }

        [JsonPropertyName("raw_url")]
        public string? RawUrl { get; set; }
    }

    public class Serie {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("year")]
        public int? Year { get; set; }

        [JsonPropertyName("begin_at")]
        public string? BeginAt { get; set; }

        [JsonPropertyName("end_at")]
        public string? EndAt { get; set; }

        [JsonPropertyName("winner_id")]
        public object? WinnerId { get; set; }

        [JsonPropertyName("slug")]
        public string? Slug { get; set; }

        [JsonPropertyName("winner_type")]
        public string? WinnerType { get; set; }

        [JsonPropertyName("modified_at")]
        public string? ModifiedAt { get; set; }

        [JsonPropertyName("league_id")]
        public int? LeagueId { get; set; }

        [JsonPropertyName("season")]
        public string? Season { get; set; }

        [JsonPropertyName("full_name")]
        public string? FullName { get; set; }
    }

    public class LiveInfo {
        [JsonPropertyName("supported")]
        public bool? Supported { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("opens_at")]
        public string? OpensAt { get; set; }
    }

    public class Game {
        [JsonPropertyName("complete")]
        public bool? Complete { get; set; }

        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("position")]
        public int? Position { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("length")]
        public int? Length { get; set; }

        [JsonPropertyName("finished")]
        public bool? Finished { get; set; }

        [JsonPropertyName("match_id")]
        public int? MatchId { get; set; }

        [JsonPropertyName("begin_at")]
        public string? BeginAt { get; set; }

        [JsonPropertyName("detailed_stats")]
        public bool? DetailedStats { get; set; }

        [JsonPropertyName("end_at")]
        public string? EndAt { get; set; }

        [JsonPropertyName("forfeit")]
        public bool? Forfeit { get; set; }

        [JsonPropertyName("winner_type")]
        public string? WinnerType { get; set; }

        [JsonPropertyName("winner")]
        public WinnerInfo? Winner { get; set; }
    }

    public class WinnerInfo {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }
    }

    public class OpponentWrapper {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("opponent")]
        public Team? Opponent { get; set; }
    }

    public class VideogameVersion {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("current")]
        public bool? Current { get; set; }
    }

    public class Tournament {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("country")]
        public object? Country { get; set; }

        [JsonPropertyName("begin_at")]
        public string? BeginAt { get; set; }

        [JsonPropertyName("detailed_stats")]
        public bool? DetailedStats { get; set; }

        [JsonPropertyName("end_at")]
        public string? EndAt { get; set; }

        [JsonPropertyName("winner_id")]
        public object? WinnerId { get; set; }

        [JsonPropertyName("slug")]
        public string? Slug { get; set; }

        [JsonPropertyName("winner_type")]
        public string? WinnerType { get; set; }

        [JsonPropertyName("serie_id")]
        public int? SerieId { get; set; }

        [JsonPropertyName("modified_at")]
        public string? ModifiedAt { get; set; }

        [JsonPropertyName("league_id")]
        public int? LeagueId { get; set; }

        [JsonPropertyName("prizepool")]
        public object? Prizepool { get; set; }

        [JsonPropertyName("tier")]
        public string? Tier { get; set; }

        [JsonPropertyName("has_bracket")]
        public bool? HasBracket { get; set; }

        [JsonPropertyName("region")]
        public string? Region { get; set; }

        [JsonPropertyName("live_supported")]
        public bool? LiveSupported { get; set; }
    }


    public class League {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("url")]
        public object? Url { get; set; }

        [JsonPropertyName("slug")]
        public string? Slug { get; set; }

        [JsonPropertyName("modified_at")]
        public string? ModifiedAt { get; set; }

        [JsonPropertyName("image_url")]
        public string? ImageUrl { get; set; }
    }

    public class Result {
        [JsonPropertyName("team_id")]
        public int? TeamId { get; set; }

        [JsonPropertyName("score")]
        public int? Score { get; set; }
        
    } 
}