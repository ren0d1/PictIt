namespace PictIt.Models
{
    public interface IModel<out T>
    {
        T Id { get; }
    }
}
