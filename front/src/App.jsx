import { useState } from 'react'

const API = '/api'

export default function App() {
    const [userId, setUserId] = useState('')
    const [topUpAmount, setTopUpAmount] = useState('')
    const [orderAmount, setOrderAmount] = useState('')
    const [orderId, setOrderId] = useState('')
    const [output, setOutput] = useState('')

    async function call(endpoint, method, body) {
        const res = await fetch(API + endpoint, {
            method,
            headers: {
                'Content-Type': 'application/json',
                'accept': '*/*',
                'X-User-Id': userId
            },
            body: body ? JSON.stringify(body) : undefined
        })

        const text = await res.text()
        setOutput(`${res.status}\n${text}`)
    }

    return (
        <div style={{ padding: 20, fontFamily: 'sans-serif' }}>
            <h2>Gozon – Test UI</h2>

            <input
                placeholder="X-User-Id (GUID)"
                value={userId}
                onChange={e => setUserId(e.target.value)}
                style={{ width: '400px' }}
            />

            <hr />

            <h3>Payments</h3>

            <button onClick={() => call('/payments/create', 'POST')}>
                Create account
            </button>

            <br /><br />

            <input
                placeholder="Top-up amount"
                value={topUpAmount}
                onChange={e => setTopUpAmount(e.target.value)}
            />

            <button onClick={() =>
                call('/payments/topup', 'POST', {
                    amount: Number(topUpAmount)
                })
            }>
                Top up
            </button>

            <br /><br />

            <button onClick={() =>
                call('/payments/balance', 'GET')
            }>
                Get balance
            </button>

            <hr />

            <h3>Orders</h3>

            <input
                placeholder="Order amount"
                value={orderAmount}
                onChange={e => setOrderAmount(e.target.value)}
            />

            <button onClick={() =>
                call('/orders', 'POST', {
                    amount: Number(orderAmount),
                    description: 'Test order'
                })
            }>
                Create order
            </button>

            <br /><br />

            <button onClick={() =>
                call('/orders', 'GET')
            }>
                Get orders
            </button>

            <br /><br />

            <input
                placeholder="OrderId (GUID)"
                value={orderId}
                onChange={e => setOrderId(e.target.value)}
                style={{ width: '400px' }}
            />

            <button onClick={() =>
                call(`/orders/${orderId}`, 'GET')
            }>
                Get order by id
            </button>

            <hr />

            <pre style={{
                background: '#111',
                color: '#0f0',
                padding: 10,
                minHeight: 200
            }}>
                {output}
            </pre>
        </div>
    )
}
