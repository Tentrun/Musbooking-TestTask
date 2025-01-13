namespace BaseServiceLibrary.DTO.ResponseDto;

/// <summary>
/// DTO ответа сервиса
/// </summary>
/// <param name="Message">json ответа</param>
/// <param name="HttpCode">КОд ответа</param>
public record ResponseDto(string Message, int HttpCode);