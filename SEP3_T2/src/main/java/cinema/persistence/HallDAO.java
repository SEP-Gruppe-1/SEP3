package cinema.persistence;

import cinema.model.Hall;

import java.sql.SQLException;
import java.util.List;

public interface HallDAO {
    Hall getHallById(int id) ;
    List<Hall> getAllHalls() ;

}
