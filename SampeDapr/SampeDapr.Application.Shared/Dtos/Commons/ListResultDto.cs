using SampeDapr.Application.Shared.Interfaces;

namespace SampeDapr.Application.Shared.Dtos
{
    [Serializable]
    public class ListResultDto<T> : IListResult<T>
    {
        private IReadOnlyList<T>? _item;
        public IReadOnlyList<T>? Items
        {
            get => _item;
            set { _item = value; }
        }
        public ListResultDto()
        {

        }
        public ListResultDto(IReadOnlyList<T> items)
        {
            _item = items;
        }
    }
}
