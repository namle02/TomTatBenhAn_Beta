using CommunityToolkit.Mvvm.ComponentModel;

namespace TomTatBenhAn_WPF.Repos.Dto
{
    public partial class PhacDoItemDTO : ObservableObject
    {
        public string _id { get; set; } = string.Empty;
        [ObservableProperty] private ProtocolDTO protocol = new();
        [ObservableProperty] private DateTime createdAt;
        [ObservableProperty] private DateTime updatedAt;

    }
    public partial class ProtocolDTO : ObservableObject
    {
        [ObservableProperty] private string name = string.Empty;
        [ObservableProperty] private string? code = null;
        [ObservableProperty] private string? source = null;
        public string raw { get; set; } = string.Empty;
        [ObservableProperty] private List<SectionDTO> sections = new();
    }

    public partial class SectionDTO : ObservableObject
    {
        [ObservableProperty] private string id = string.Empty;
        [ObservableProperty] private string title = string.Empty;
        [ObservableProperty] private List<string> content = new();
        [ObservableProperty] private List<SectionDTO> children = new();
    }

    public partial class ApiResponseDTO<T> : ObservableObject
    {
        [ObservableProperty] private bool success;
        [ObservableProperty] private string message = string.Empty;
        [ObservableProperty] private T? data;
        [ObservableProperty] private string? mongoId;
        [ObservableProperty] private string? action;
        [ObservableProperty] private string? error;
        [ObservableProperty] private string? existingId;
        [ObservableProperty] private bool exists;
        [ObservableProperty] private PhacDoItemDTO? existingProtocol;
    }

    public partial class AddPhacDoRequestDTO : ObservableObject
    {
        [ObservableProperty] private string rawtext = string.Empty;
        [ObservableProperty] private bool force = false;
    }
}
