import React, { useEffect, useState } from 'react'
import { createUser, getUserById, updateUser } from '../api'

export default function UserForm({ userId = null, onSaved = null }) {
  const [firstName, setFirstName] = useState('')
  const [lastName, setLastName] = useState('')
  const [email, setEmail] = useState('')
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState(null)

  useEffect(() => {
    if (!userId) {
      setFirstName(''); setLastName(''); setEmail(''); return
    }
    let mounted = true
    ;(async () => {
      setLoading(true)
      try {
        const u = await getUserById(userId)
        if (!mounted) return
        setFirstName(u.firstName ?? '')
        setLastName(u.lastName ?? '')
        setEmail(u.email ?? '')
      } catch (err) {
        setError(err.message)
      } finally {
        if (mounted) setLoading(false)
      }
    })()
    return () => { mounted = false }
  }, [userId])

  const reset = () => { setFirstName(''); setLastName(''); setEmail(''); setError(null) }

  const handleSubmit = async (e) => {
    e.preventDefault()
    setError(null)
    setLoading(true)
    const payload = { firstName, lastName, email }
    try {
      if (userId) {
        await updateUser(userId, payload)
        alert('Usuario actualizado')
      } else {
        await createUser(payload)
        alert('Usuario creado')
        reset()
      }
      if (typeof onSaved === 'function') onSaved()
    } catch (err) {
      // intenta parsear mensaje de validación si viene en JSON
      setError(err.message)
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="user-form">
      <h2>{userId ? 'Editar Usuario' : 'Crear Usuario'}</h2>
      {error && <p className="error">{error}</p>}
      <form onSubmit={handleSubmit}>
        <label>
          First Name
          <input value={firstName} onChange={e => setFirstName(e.target.value)} />
        </label>
        <label>
          Last Name
          <input value={lastName} onChange={e => setLastName(e.target.value)} />
        </label>
        <label>
          Email
          <input value={email} onChange={e => setEmail(e.target.value)} />
        </label>
        <div className="actions">
          <button type="submit" disabled={loading}>{loading ? 'Guardando...' : (userId ? 'Actualizar' : 'Crear')}</button>
          {userId && <button type="button" onClick={() => { if (typeof onSaved === 'function') onSaved() }}>Cancelar</button>}
        </div>
      </form>
    </div>
  )
}
