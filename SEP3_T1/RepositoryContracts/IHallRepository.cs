using Entities;

namespace RepositoryContract;

public interface IHallRepository
{
    List<Seat> GetSelectedSeats();


    public List<Seat> GetAvailableSeats();

    public string GetBookedSeatsDisplay();

    public Seat GetSeat(char row, int number);

    Task<Hall> getHallbyidAsync(int id);
    IQueryable<Hall> GetAll();
}