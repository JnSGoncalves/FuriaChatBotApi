using System;
using System.Globalization;

namespace FuriaChatBotApi.Helpers {

    public static class DateTimeHelpers {
        /// <summary>
        /// Converte uma string UTC ISO 8601 para o horário de Brasília e formata como "no dia DD/MM/AAAA às HH:MM".
        /// </summary>
        /// <param name="utcString">String no formato "yyyy-MM-ddTHH:mm:ssZ", ex: "2025-05-10T08:00:00Z".</param>
        /// <returns>Texto formatado: "no dia DD/MM/AAAA às HH:MM".</returns>
        public static string FormatBrasiliaTime(string utcString) {
            if (!DateTime.TryParseExact(
                    utcString,
                    "yyyy-MM-ddTHH:mm:ssZ",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                    out DateTime utcDateTime)) {
                throw new ArgumentException("String não está em formato UTC ISO 8601 válido.", nameof(utcString));
            }

            // Obtém o fuso de Brasília wo Windows:
            TimeZoneInfo brasiliaZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");

            DateTime brasiliaTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, brasiliaZone);

            return $"no dia {brasiliaTime:dd/MM/yyyy} às {brasiliaTime:HH:mm}";
        }
    }

}
