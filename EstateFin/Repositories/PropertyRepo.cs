using EstateFin.Models;

namespace EstateFin.Repositories
{
    public interface PropertyRepo
    {
        List<Property> GetProperties(int id);
        List<Property_Type> dropdown();

        List<string> propertyfile(Bind prop);
        void add_property(Bind prop);
        void delete_properties(int id);

        Bind edit_property(int id);

        void edit_property_post(Bind e);
        void property_typess(Property_Type e);
        void delete_property_typess(int id);

        void edit_property_typess(Property_Type e);

    }
}