package cinema.persistence;

import cinema.model.Layout;

import java.util.List;

public interface LayoutDAO {

    Layout getLayoutById(int layoutId);
    List<Layout> getAllLayouts();
}
