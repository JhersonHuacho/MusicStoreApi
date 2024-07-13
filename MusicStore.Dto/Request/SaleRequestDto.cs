using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Dto.Request;

// los records son inmutables y se usan para almacenar datos
// no se pueden modificar una vez creados
// se usan para almacenar datos
// se recomienda usar reord para DTO
public record SaleRequestDto(
	int ConcertId,
	short TicketsQuantity);
