using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IndicatorsManager.Domain;

namespace IndicatorsManager.WebApi.Models
{
    public class IndicatorModel : Model<Indicator, IndicatorModel>
    {  
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }

        public List<string> ActiveColours{ get; set; }

        public string AreaName { get; set; }

        public IndicatorModel()
        {
        }

        public IndicatorModel(Indicator entity)
        {
            SetModel(entity);
        }


        public override Indicator ToEntity() => new Indicator()
        {
            Id = this.Id,
            Name = this.Name
        };

        protected override IndicatorModel SetModel(Indicator entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            if(entity.Area != null)
                AreaName = entity.Area.Name;
            
            List<string> colours = entity.GetActiveColours();
            if(entity.Conditions.Count != 0 && colours != null && colours.Count != 0)
                ActiveColours = colours;
                
            return this;
        }
    }
}