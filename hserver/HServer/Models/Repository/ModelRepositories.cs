﻿using HServer.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HServer.Models.Repository
{
    public class TipRepository : Repository<Tip>
    {
        public TipRepository()
            : this(new DataBaseContext())
        {
        }
        public TipRepository(DbContext context)
            : base(context)
        {
        }

        public IEnumerable<Tip> GetByCategoryId(int id)
        {
            return this.Find(t => t.CategoryId == id);
        }
    }

    public class TipCategoryRepository : Repository<TipCategory>
    {
        public TipCategoryRepository()
            : this(new DataBaseContext())
        {
        }
        public TipCategoryRepository(DbContext context)
            : base(context)
        {
        }

        public IEnumerable<TipCategory> GetIgerly()
        {
            return this._context.Set<TipCategory>().Include("Tips").AsEnumerable();
        }
    }

    public class DoctorRepository : Repository<Doctor>
    {
        public DoctorRepository()
            : this(new DataBaseContext())
        {
        }
        public DoctorRepository(DbContext context)
            : base(context)
        {
        }

        public IEnumerable<Doctor> GetIgerly()
        {
            return this._context.Set<Doctor>()
                .Include("Position")
                .Include("Department")
                .Include("SubDepartment")
                .Include("Languages")
                .Include("Qualifications").AsEnumerable();
        }
    }

    public class DepartmentRepository : Repository<Position>
    {
        public DepartmentRepository(DbContext context)
            : base(context)
        {
        }
        public DepartmentRepository()
            : this(new DataBaseContext())
        {
        }
    }
}