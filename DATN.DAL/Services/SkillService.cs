using DATN.DAL.Context;
using DATN.DAL.Models;
using DATN.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.DAL.Services
{
    public class SkillService : BaseService
    {
        public SkillService(DatabaseContext context) : base(context)
        {
        }
        public List<string> GetSuggesstion_Skill(string q)
        {
            var skills = new List<string>();

            try
            {
                var query = (from p in context.skill
                             where p.ten_skill != null
                             select new Skill { ten_skill = p.ten_skill }
                            ).Distinct().ToList();

                foreach (Skill item in query)
                {
                    if ((skills.Count() < 5) && (StringUtils.RemoveVietnameseUnicode(item.ten_skill).StartsWith(StringUtils.RemoveVietnameseUnicode(q)) == true))

                        skills.Add(item.ten_skill);
                }
                skills.Sort();

                return skills;

            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        public bool CheckExistSkill(string q)
        {
            bool result = false;
            var skillTable = (from p in context.skill
                              select new Skill
                              {
                                  ma_skill = p.ma_skill,
                                  ten_skill = p.ten_skill
                              });

            foreach (var data in skillTable)
            {
                result =  (StringUtils.RemoveVietnameseUnicode(q) == StringUtils.RemoveVietnameseUnicode(data.ten_skill)) ? true : false;

            }

            return result;
        }

        public int GetIdSkill(string q)
        {
            var dataTable = (from p in context.skill
                             select new Skill
                             {
                                 ma_skill = p.ma_skill,
                                 ten_skill = p.ten_skill
                             });


            int result = -1;
            foreach (var data in dataTable)
            {
                 result = (StringUtils.RemoveVietnameseUnicode(q) == StringUtils.RemoveVietnameseUnicode(data.ten_skill)) ? data.ma_skill : -1;

            }
            return result;
        }
    }
}
