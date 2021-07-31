using FA.JustBlog.Models.Common;
using FA.JustBlog.Services.BaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.JustBlog.Services
{
    public interface ITagServices : IBaseService<Tag>
    {
        Tag GetTagByUrlSlug(string urlSlug);

        Task<Tag> GetTagByUrlSlugAsync(string urlSlug);

        Task<IEnumerable<Tag>> GetPopularTags(int size = 10);
    }
}
