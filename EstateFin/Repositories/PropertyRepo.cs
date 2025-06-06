﻿using EstateFin.Models;

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

        List<Property> search(string txt);
        List<Property> FetchAllProperty();
        List<Property_Type> FetchAllPropertyType();
        Property_Type Edit_Property_Type(int id);
        List<Property> Property_User_List();
        Property property_user_findbyid(int id);
        List<Property> Property_Tenant_List();
        public double get_emi(double loan_amount, double interest, int year);

    }
}