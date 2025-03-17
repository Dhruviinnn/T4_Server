import React from 'react'
import { timetableImages } from '../../public/images';

const Images = () => {
    return (
        <div className="hidden lg:block w-1/2 bg-gradient-to-br from-white/5 via-white/3 to-transparent relative overflow-hidden">
            <div className="absolute inset-0 bg-black/40" />
            {timetableImages.map((image, index) => (
                <div
                    key={index}
                    className="absolute w-[500px] h-[300px] rounded-xl overflow-hidden"
                    style={{
                        top: `${Math.random() * 60}%`,
                        left: `${Math.random() * 60}%`,
                        transform: image.transform,
                        transition: 'all 0.5s ease-out',
                    }}
                >
                    <div
                        className="w-full h-full bg-cover bg-center opacity-10 hover:opacity-20 transition-opacity duration-300"
                        style={{ backgroundImage: `url(${image.url})` }}
                    />
                </div>
            ))}
        </div>
    )
}

export default Images