import React from 'react'

const Toaster = () => {
    return (
        <Toaster
            position="bottom-right"
            toastOptions={{
                style: {
                    fontSize: '1rem',
                    background: 'rgb(239 68 68)',
                    color: 'white',
                    border: 'none'
                },
                className: 'bg-red-500'
            }}
        />
    )
}

export default Toaster