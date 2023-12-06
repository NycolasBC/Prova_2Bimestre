﻿using AutoMapper;
using Revisao.Application.Interfaces;
using Revisao.Application.ViewModels;
using Revisao.Domain.Entities;
using Revisao.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revisao.Application.Services
{
	public class RegistroJogoService : IRegistroJogoService
	{
		private readonly IRegistroJogoRepository _registroJogoRepository;
		private readonly IMapper _mapper;

		public RegistroJogoService(IRegistroJogoRepository registroJogoRepository, IMapper mapper)
		{
			_registroJogoRepository = registroJogoRepository;
			_mapper = mapper;
		}

		public async Task<IEnumerable<RegistroJogoViewModel>> ObterTodosOsJogos()
		{
			return _mapper.Map<IEnumerable<RegistroJogoViewModel>>(await _registroJogoRepository.ObterTodosOsJogos());

		}

		public async Task RegistrarJogo(NovoRegistroJogoViewModel registroJogoViewModel)
		{
            if (ValidacaoNumeroDaSorte(registroJogoViewModel))
            {
                throw new Exception("Os números devem ser diferentes um do outro");
            }

            await _registroJogoRepository.RegistrarJogo(_mapper.Map<RegistroJogo>(registroJogoViewModel));
		}


        #region Validação Número da sorte

        private bool ValidacaoNumeroDaSorte(NovoRegistroJogoViewModel registroJogoViewModel)
        {
            int[] arrayDeJogos = new int[]
            {
                registroJogoViewModel.primeiroNumero,
                registroJogoViewModel.segundoNumero,
                registroJogoViewModel.terceiroNumero,
                registroJogoViewModel.quartoNumero,
                registroJogoViewModel.quintoNumero,
                registroJogoViewModel.sextoNumero
            };

            HashSet<int> numerosVistos = new HashSet<int>();

            foreach (int numero in arrayDeJogos)
            {
                if (numerosVistos.Contains(numero))
                {
                    return true;
                }
                else
                {
                    numerosVistos.Add(numero);
                }
            }

            return false;
        }

        #endregion
    }
}
