using System;

namespace WorkingStandards.Entities.External
{
	/// <summary>
	/// Цеха предприятия
	/// </summary>
	public class WorkGuild : IComparable<WorkGuild>
	{
		public decimal Id { get; set; }

		public int CompareTo(WorkGuild other)
		{
		    if (ReferenceEquals(this, other))
		    {
		        return 0;
		    }

		    if (ReferenceEquals(null, other))
		    {
		        return 1;
		    }
			return Id.CompareTo(other.Id);
		}

		protected bool Equals(WorkGuild other)
		{
		    {
		        return Id == other.Id;
		    }
		}

		public override bool Equals(object obj)
		{
		    if (ReferenceEquals(null, obj))
		    {
		        return false;
		    }

		    if (ReferenceEquals(this, obj))
		    {
		        return true;
		    }

		    if (obj.GetType() != this.GetType())
		    {
		        return false;
		    }
			return Equals((WorkGuild) obj);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}
