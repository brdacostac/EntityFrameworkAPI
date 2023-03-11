using Entity_framework.Enums;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Model.RunePage;

namespace BiblioMilieu.Mapper.EnumsMapper
{
    public static class CategoryMapperEnum
    {
        public static CategoryDb ToDb(this Category category)
        {
            switch (category)
            {
                case Category.OtherMinor1:
                    return CategoryDb.OtherMinor1;
                case Category.OtherMinor2:
                    return CategoryDb.OtherMinor2;
                case Category.Minor2:
                    return CategoryDb.Minor2;
                case Category.Minor1:
                    return CategoryDb.Minor1;
                case Category.Minor3:
                    return CategoryDb.Minor3;
                case Category.Major:
                    return CategoryDb.Major;       
                default:
                    return CategoryDb.Major;
            }
        }
        public static Category ToRuneFamily(this CategoryDb categoryDb)
        {
            switch (categoryDb)
            {
                case CategoryDb.OtherMinor1:
                    return Category.OtherMinor1;
                case CategoryDb.OtherMinor2:
                    return Category.OtherMinor2;
                case CategoryDb.Minor2:
                    return Category.Minor2;
                case CategoryDb.Minor1:
                    return Category.Minor1;
                case CategoryDb.Minor3:
                    return Category.Minor3;
                case CategoryDb.Major:
                    return Category.Major;
                default:
                    return Category.Major;
            }
        }
    }
}
