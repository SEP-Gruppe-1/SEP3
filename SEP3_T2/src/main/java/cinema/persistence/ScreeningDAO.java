package cinema.persistence;

import cinema.model.Screening;

import java.sql.SQLException;
import java.util.List;

public interface ScreeningDAO {

    List<Screening> getAllScreenings();
    Screening getScreeningById(int id);
    void addScreening(Screening screening) throws SQLException;
}
