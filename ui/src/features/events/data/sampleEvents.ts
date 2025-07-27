import { v4 as uuidv4 } from 'uuid'

export const sampleEvents = [
    {
        id: uuidv4(),
        title: 'Web Development Summit 2024',
        description: 'Join industry experts for an intensive two-day conference covering the latest in web development, including React, Vue, Node.js, and cloud architecture. Network with leading developers and participate in hands-on workshops.',
        startDate: '2024-06-15T09:00:00',
        endDate: '2024-06-16T17:00:00',
        venueName: 'Tech Innovation Center',
        totalSeats: 500,
        availableSeats: 127,
        price: 599.99,
        createdAt: '2024-01-15T08:00:00'
      },
      {
        id: uuidv4(),
        title: 'Summer Jazz Festival',
        description: 'Experience three magical evenings of world-class jazz performances under the stars. Featuring Grammy-winning artists, local talents, and fusion bands. Food vendors and wine tasting included.',
        startDate: '2024-07-20T17:00:00',
        endDate: '2024-07-22T23:00:00',
        venueName: 'Riverside Amphitheater',
        totalSeats: 2000,
        availableSeats: 856,
        price: 175.00,
        createdAt: '2024-01-20T10:30:00'
      },
      {
        id: uuidv4(),
        title: 'AI & Machine Learning Workshop',
        description: 'Practical workshop on implementing AI solutions. Topics include deep learning, neural networks, and real-world applications. Includes hands-on sessions with industry-standard tools and take-home projects.',
        startDate: '2024-05-10T08:30:00',
        endDate: '2024-05-10T16:30:00',
        venueName: 'Digital Learning Hub',
        totalSeats: 100,
        availableSeats: 12,
        price: 349.99,
        createdAt: '2024-01-25T14:20:00'
      },
      {
        id: uuidv4(),
        title: 'Startup Networking Breakfast',
        description: 'Monthly breakfast meetup for startup founders, investors, and tech entrepreneurs. Features lightning talks, pitch practice, and structured networking sessions.',
        startDate: '2024-04-05T07:30:00',
        endDate: '2024-04-05T10:00:00',
        venueName: 'Innovation Lounge',
        totalSeats: 75,
        availableSeats: 25,
        price: 45.00,
        createdAt: '2024-01-28T09:15:00'
      },
      {
        id: uuidv4(),
        title: 'Digital Marketing Masterclass',
        description: 'Comprehensive one-day course covering SEO, social media marketing, content strategy, and analytics. Learn from successful campaign managers and digital marketing experts.',
        startDate: '2024-05-25T09:00:00',
        endDate: '2024-05-25T17:00:00',
        venueName: 'Business Center Downtown',
        totalSeats: 150,
        availableSeats: 83,
        price: 299.99,
        createdAt: '2024-02-01T11:00:00'
      },
      {
        id: uuidv4(),
        title: 'Cybersecurity Conference 2024',
        description: 'Annual cybersecurity conference featuring keynotes from leading security experts, workshops on threat detection, and the latest in security technologies. Includes certification preparation tracks.',
        startDate: '2024-08-15T08:00:00',
        endDate: '2024-08-17T18:00:00',
        venueName: 'Metropolitan Convention Center',
        totalSeats: 800,
        availableSeats: 342,
        price: 899.99,
        createdAt: '2024-02-05T13:45:00'
      },
      {
        id: uuidv4(),
        title: 'Product Management Bootcamp',
        description: 'Intensive three-day bootcamp for aspiring and junior product managers. Learn product strategy, user research, roadmap planning, and agile methodologies through real-world case studies.',
        startDate: '2024-06-03T09:00:00',
        endDate: '2024-06-05T17:00:00',
        venueName: 'Product Innovation Campus',
        totalSeats: 120,
        availableSeats: 47,
        price: 1299.99,
        createdAt: '2024-02-10T15:30:00'
      }
]