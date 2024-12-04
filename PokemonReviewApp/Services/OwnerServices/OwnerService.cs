using AutoMapper;
using FluentValidation;
using PokemonReviewApp.Data;
using PokemonReviewApp.Dtos.CategoryDtos;
using PokemonReviewApp.Dtos.OwnerDtos;
using PokemonReviewApp.Dtos.PokemonDtos;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace PokemonReviewApp.Services.OwnerServices
{
    public class OwnerService : IOwnerService
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IPokemonOwnerRepository _pokemonOwnerRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<OwnerCreateDto> _ownerValidator;
        private readonly IValidator<PokemonCreateDto> _pokemonValidator;

        public OwnerService(IOwnerRepository ownerRepository, IMapper mapper, IPokemonRepository pokemonRepository,IPokemonOwnerRepository pokemonOwnerRepository, IValidator<OwnerCreateDto> ownerValidator,
        IValidator<PokemonCreateDto> pokemonValidator)
        {
            _ownerRepository = ownerRepository;
            _mapper = mapper;
            _pokemonRepository = pokemonRepository;
            _pokemonOwnerRepository = pokemonOwnerRepository;
            _ownerValidator = ownerValidator;
            _pokemonValidator = pokemonValidator;
        }

        public ICollection<OwnerDto> GetOwners()
        {
            var owners = _ownerRepository.GetOwners();
            return _mapper.Map<ICollection<OwnerDto>>(owners);
        }

        public OwnerDto GetOwnerById(int ownerId)
        {
            var owner = _ownerRepository.GetOwner(ownerId);
            return _mapper.Map<OwnerDto>(owner);
        }

        public ICollection<OwnerDto> GetOwnersOfPokemon(int pokeId)
        {
            var owners = _ownerRepository.GetOwnerOfAPokemon(pokeId);
            return _mapper.Map<ICollection<OwnerDto>>(owners);
        }

        public ICollection<PokemonDto> GetPokemonsByOwner(int ownerId)
        {
            var pokemons = _ownerRepository.GetPokemonByOwner(ownerId);
            return _mapper.Map<ICollection<PokemonDto>>(pokemons);
        }

        public bool OwnerExists(int ownerId)
        {
            return _ownerRepository.OwnerExists(ownerId);
        }
        public bool CreateOwner(OwnerCreateDto ownerCreateDto)
        {
            // DTO'dan Owner modeline dönüştürme
            var owner = _mapper.Map<Owner>(ownerCreateDto);

            // Repository'ye Owner modelini gönderiyoruz
            return _ownerRepository.CreateOwner(owner);
        }
        // Transaction ile hem Owner hem Pokemon oluşturma işlemi
        public bool CreateOwnerWithPokemon(OwnerCreateDto ownerCreateDto, PokemonCreateDto pokemonCreateDto)
        {
            // OwnerCreateDto için doğrulama
            var ownerValidationResult = _ownerValidator.Validate(ownerCreateDto);
            if (!ownerValidationResult.IsValid)
            {
                // örneğin loglama yapabilir
                return false; 
            }

            // PokemonCreateDto için doğrulama
            var pokemonValidationResult = _pokemonValidator.Validate(pokemonCreateDto);
            if (!pokemonValidationResult.IsValid)
            {
                //  loglama yapabilirsiniz
                return false; 
            }

            using (var transaction = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    // DTO'dan Owner modeline dönüştürme
                    var owner = _mapper.Map<Owner>(ownerCreateDto);

                    // Owner kaydet
                    if (!_ownerRepository.CreateOwner(owner))
                    {
                        return false;
                    }

                    // DTO'dan Pokemon modeline dönüştürme
                    var pokemon = _mapper.Map<Pokemon>(pokemonCreateDto);

                    // Pokemon kaydet
                    if (!_pokemonRepository.CreatePokemon(owner.Id, pokemonCreateDto.CategoryId, pokemon))
                    {
                        return false;
                    }

                    // Transaction commit
                    transaction.Complete();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool UpdateOwner(int ownerId, OwnerUpdateDto ownerUpdateDto)
        {
            // Mevcut owner'ı al
            var existingOwner = _ownerRepository.GetOwner(ownerId);
            if (existingOwner == null) return false;

            // Güncellenmiş verileri mevcut owner'a yansıt
            _mapper.Map(ownerUpdateDto, existingOwner);

            // Güncellenmiş owner'ı kaydet
            return _ownerRepository.UpdateOwner(existingOwner);
        }

        public bool DeleteOwner(int ownerId)
        {
            // Belirtilen owner'ın var olup olmadığını kontrol et
            if (!_ownerRepository.OwnerExists(ownerId)) return false;

            // Owner'ı sil
            return _ownerRepository.DeleteOwner(ownerId);
        }
    }
}
